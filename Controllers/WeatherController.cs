using Microsoft.AspNetCore.Mvc;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private WeatherManager weatherManager;
    private ILogger logger;
    public WeatherController(IConfiguration configuration, ILogger log){
        logger = log;
        weatherManager = new WeatherManager(configuration, logger);
    }

    [HttpGet]
    public async Task<ActionResult<WeatherItem>> Get([FromQuery] decimal? lat, [FromQuery] decimal? lon){
        if (lat == null || lon == null) {return NotFound();}

        //round lat/lon because weather api only deals with 4 decimals
        lat = Math.Round(lat.Value, 4, MidpointRounding.ToZero);
        lon = Math.Round(lon.Value, 4, MidpointRounding.ToZero);

        var data = await weatherManager.GetWeather(lat.Value, lon.Value);

        return Ok(data);
        
    }
}