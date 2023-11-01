using System.Net.Http.Headers;
using KioskApi.Models;

namespace KioskApi.Managers;
public class WeatherManager
{
    private DatabaseManager dbm;
    private IConfiguration Configuration{get;}
    private ILogger logger;
    public WeatherManager(IConfiguration configuration, ILogger log){
        Configuration = configuration;
        logger = log;
        dbm = new DatabaseManager(configuration, logger);
    }
    public async Task<WeatherItem> GetWeather(decimal lat, decimal lon){

        DateTime threeMinutesAgo = DateTime.Now.AddMinutes(-3);
        WeatherItem ReturnData = new WeatherItem();
        var weatherData = await dbm.GetWeatherData(100);
        
        //get cached data
        var data = weatherData.Where(x => x.Lat == lat && x.Lon == lon).ToList().FirstOrDefault();

        // if the data does not exist or is old we need to refresh the data
        if (data == null || data.LastRefreshed  < threeMinutesAgo) {

            if (data == null) { 
                data = new WeatherItem();
            }
            
            var rawWeatherData = await GetRawWeatherAsync(lat, lon);
            data.ProcessRawWeatherData(rawWeatherData);

            dbm.AddUpdateData(data);
            
        } else{
            data.Type = "cached";
        }

        return data;
    }
    #region "Open Weather API Access"
    private async Task<string> GetRawWeatherAsync(decimal lat, decimal lon)
    {
        string result = "[{}]";
        
        using(var client = new HttpClient())
        {
            string uri = String.Format(Configuration["WeatherApi:weather_req_url"] ?? string.Empty, 
                Configuration["WeatherApi:weather_api_token"], 
                lat, 
                lon, 
                Configuration["WeatherApi:weather_lang"], 
                Configuration["WeatherApi:weather_unit"], 
                Configuration["WeatherApi:weather_exclude_list"]);

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //GET Method
            HttpResponseMessage response = await client.GetAsync(uri);
            
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorMessage = string.Format($"Error Getting data from {uri}. Status Code: {response.StatusCode}");
                throw new Exception(errorMessage);
            }
        }

        return result;
    } 
    #endregion

    
}