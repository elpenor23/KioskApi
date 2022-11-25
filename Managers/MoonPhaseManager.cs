using KioskApi.Models;

namespace KioskApi.Managers;
public class MoonPhaseManager
{
    private IConfiguration Configuration { get; }
    private WeatherManager weatherManager;
    private ILogger logger;
    public MoonPhaseManager( IConfiguration configuration, ILogger log)
    {
        Configuration = configuration;
        logger = log;
        weatherManager = new WeatherManager(configuration, logger);
    }

    public async Task<MoonData> GetMoonPhase(decimal lat, decimal lon)
    {
        var weather = await weatherManager.GetWeather(lat, lon);

        //check for nulls so it stops warning me
        if (weather.MoonPhase == null || weather.SunriseTime == null || weather.SunsetTime == null) {return new MoonData(4, "Full Moon", "full-moon", "24 Hours");}

        var moonItem = GetMoonData(weather.MoonPhase.Value, weather.SunriseTime.Value, weather.SunsetTime.Value);

        return moonItem;
    }

    private MoonData GetMoonData(decimal moonPhase, DateTime sunrise, DateTime sunset){
        var index = GetPhaseIndex(moonPhase);
        var data = GetPhaseData(index);
        var dayLength = GetDayLength(sunrise, sunset);

        return new MoonData(index, data.Item1, data.Item2, dayLength);
    }

    private int GetPhaseIndex(decimal moonPhase)
    {
        int index = 0;
        if (moonPhase == 0 || moonPhase == 1) {
            index = 0;
        } else if (moonPhase > 0 && moonPhase < 0.25M) {
            index = 1;
        } else if (moonPhase == 0.25M) {
            index = 2;
        } else if (moonPhase > 0.25M && moonPhase < 0.5M) {
            index = 3;
        } else if (moonPhase == 0.5M) {
            index = 4;
        } else if (moonPhase > 0.5M && moonPhase < 0.75M) {
            index = 5;
        } else if (moonPhase == 0.75M) {
            index = 6;
        } else if (moonPhase > 0.75M && moonPhase < 1M) {
            index = 7;
        } else {
            index = 0;
        }

        return index;
    }

    private Tuple<string, string> GetPhaseData(int moonIndex)
    {
        string icon = string.Empty;
        string name = string.Empty;

        switch (moonIndex)
        {
            case 0:
                icon = "new-moon";
                name = "New Moon";
                break;
            case 1:
                icon =  "waxing-crescent-moon";
                name = "Waxing Crescent Moon";
                break;
            case 2:
                icon = "first-quarter-moon";
                name = "First Quarter Moon";
                break;
            case 3:
                icon = "waxing-gibbous-moon";
                name = "Waxing Gibbous Moon";
                break;
            case 4:
                icon = "full-moon";
                name = "Full Moon";
                break;
            case 5:
                icon = "waning-gibbous-moon";
                name = "Waning Gibbous Moon";
                break;
            case 6:
                icon = "last-quarter-moon";
                name = "Last Quarter Moon";
                break;
            case 7:
                icon = "waning-crescent-moon";
                name = "Waning Crescent Moon";
                break;
        }

        return new Tuple<string, string>(name, icon);
    }

    private string GetDayLength(DateTime sunrise, DateTime sunset){
        TimeSpan span = (sunset - sunrise);

        return String.Format("{0} hours, {1} minutes", span.Hours, span.Minutes);
    }
}