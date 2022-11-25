using KioskApi.Enums;

namespace KioskApi.Models;

public class WeatherItem : IModel
{
    const string degree_sign = "°";

    [SQLite.PrimaryKey]
    public string? Id { get; set; }
    public decimal? Lat { get; set; }
    
    public decimal? Lon { get; set; }

    public DateTime? WeatherTime { get; set; }
    public string? WeatherTimeFormatted
    {
        get
        {
            return WeatherTime?.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
    public int? CurrentTemp { get; set; }
    public string? CurrentTempFormatted
    {
        get
        {
            return string.Format("{0}{1}", CurrentTemp.ToString(), degree_sign);
        }
    }
    public int? CurrentDewPoint { get; set; }
    public int? CurrentFeelsLike { get; set; }
    public string? CurrentFeelsLikeFormatted
    {
        get
        {
            return string.Format("{0}{1}", CurrentFeelsLike.ToString(), degree_sign);
        }
    }
    public int? CurrentWindSpeed { get; set; }
    public string? CurrentIconId { get; set; }
    public string? CurrentMain { get; set; } //TODO: GIVE THIS A BETTER NAME
    public int? TodayMinTemp { get; set; }
    public string? TodayMinTempFormatted
    {
        get
        {
            return string.Format("{0}{1}", TodayMinTemp.ToString(), degree_sign);
        }
    }
    public int? TodayMaxTemp { get; set; }
    public string? TodayMaxTempFormatted
    {
        get
        {
            return string.Format("{0}{1}", TodayMaxTemp.ToString(), degree_sign);
        }
    }
    public string? TodayDescription { get; set; }
    public string? TodaySummary
    {
        get
        {
            return string.Format("Today: {0}\n Low: {1} / High: {2}", TodayDescription, TodayMinTemp, TodayMaxTemp);
        }
    }
    public int? TommorrowMinTemp { get; set; }
    public string? TommorrowMinTempFormatted
    {
        get
        {
            return string.Format("{0}{1}", TommorrowMinTemp.ToString(), degree_sign);
        }
    }
    public int? TommorrowMaxTemp { get; set; }
    public string? TommorrowMaxTempFormatted
    {
        get
        {
            return string.Format("{0}{1}", TommorrowMaxTemp.ToString(), degree_sign);
        }
    }
    public string? TommorrowDescription { get; set; }
    public string? TommorrowForecast
    {
        get
        {
            return string.Format("Tomorrow: {0}\nLow: {1} / High: {2}", TommorrowDescription, TommorrowMinTempFormatted, TommorrowMaxTempFormatted);
        }
    }

    public DateTime? SunriseTime { get; set; }
    public DateTime? SunsetTime { get; set; }
    public decimal? MoonPhase { get; set; }
    public string? Type { get; set; }
    public DateTime? LastRefreshed { get; set; }
    public TimeOfDay TimeOfDay
    {
        get
        {
            return this.GetTimeOfDay();
        }
    }

    public void ProcessRawWeatherData(string rawWeatherData)
    {
        dynamic? jsonWeatherData = Newtonsoft.Json.JsonConvert.DeserializeObject(rawWeatherData);

        //if no data is passed in give up
        if (jsonWeatherData == null) { return; }
        this.LastRefreshed = DateTime.Now;
        this.Lat = Math.Round((decimal)jsonWeatherData["lat"].Value, 4, MidpointRounding.ToZero);
        this.Lon = Math.Round((decimal)jsonWeatherData["lon"].Value, 4, MidpointRounding.ToZero);

        this.Id = this.Lat.ToString() + this.Lon.ToString();

        this.CurrentTemp = (int)jsonWeatherData["current"]["temp"];

        this.CurrentDewPoint = (int)jsonWeatherData["current"]["dew_point"];

        // #format temps for display
        var feelslike = jsonWeatherData["current"]["feels_like"];
        this.CurrentFeelsLike = (int)Math.Round(feelslike.Value, 0);

        this.TodayMinTemp = (int)jsonWeatherData["daily"][0]["temp"]["min"];
        this.TodayMaxTemp = (int)jsonWeatherData["daily"][0]["temp"]["max"];

        // #summary and forecast
        this.TodayDescription = jsonWeatherData["current"]["weather"][0]["description"];

        this.TommorrowMinTemp = jsonWeatherData["daily"][1]["temp"]["min"];
        this.TommorrowMaxTemp = jsonWeatherData["daily"][1]["temp"]["max"];

        this.TommorrowDescription = jsonWeatherData["daily"][1]["weather"][0]["description"];

        this.CurrentIconId = jsonWeatherData["current"]["weather"][0]["icon"];

        this.CurrentMain = jsonWeatherData["current"]["weather"][0]["main"];

        this.WeatherTime = ConvertEpochTimeToDateTime(jsonWeatherData["current"]["dt"].Value);

        this.SunriseTime = ConvertEpochTimeToDateTime(jsonWeatherData["current"]["sunrise"].Value);
        this.SunsetTime = ConvertEpochTimeToDateTime(jsonWeatherData["current"]["sunset"].Value);

        this.MoonPhase = jsonWeatherData["daily"][0]["moon_phase"];

        this.CurrentWindSpeed = jsonWeatherData["current"]["wind_speed"];

        this.Type = "refreshed";

        return;
    }

    private DateTime ConvertEpochTimeToDateTime(Int64 timestamp)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        return dtDateTime.AddSeconds(timestamp).ToLocalTime();
    }

    private TimeOfDay GetTimeOfDay()
    {
        if (this.WeatherTime == null || this.SunriseTime == null || this.SunsetTime == null){
            return TimeOfDay.Unknown; 
        }
        
        if (isDawn(this.WeatherTime.Value, this.SunriseTime.Value))
        {
            return TimeOfDay.Day;
        }
        else if (isDusk(this.WeatherTime.Value, this.SunsetTime.Value))
        {
            return TimeOfDay.Dusk;
        }
        else if (isDay(this.WeatherTime.Value, this.SunriseTime.Value, this.SunsetTime.Value))
        {
            return TimeOfDay.Day;
        }
        else if (isNight(this.WeatherTime.Value, this.SunsetTime.Value))
        {
            return TimeOfDay.Night;
        }
        else
        {
            return TimeOfDay.Unknown;
        }
    }


    private static bool isDay(DateTime currentTime, DateTime sunRiseTime, DateTime sunSetTime)
    {
        return (sunRiseTime <= currentTime && currentTime <= sunSetTime);
    }

    private static bool isDawn(DateTime currentTime, DateTime sunRiseTime)
    {
        var thirtyMinutesBeforeSunrise = sunRiseTime.AddMinutes(-30);
        var thirtyMinutesAfterSunrise = sunRiseTime.AddMinutes(30);

        return (thirtyMinutesBeforeSunrise <= currentTime && currentTime <= thirtyMinutesAfterSunrise);
    }

    private static bool isDusk(DateTime currentTime, DateTime sunSetTime)
    {
        var thirtyMinutesBeforeSunset = sunSetTime.AddMinutes(-30);
        var thirtyMinutesAfterSunset = sunSetTime.AddMinutes(30);

        return (thirtyMinutesBeforeSunset <= currentTime && currentTime <= thirtyMinutesAfterSunset);
    }

    private static bool isNight(DateTime currentTime, DateTime sunSetTime)
    {
        return (currentTime >= sunSetTime);
    }
}