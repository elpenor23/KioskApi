using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    WeatherManager weatherManager;
    public WeatherController(IDocumentClient documentClient, IConfiguration configuration){
        weatherManager = new WeatherManager(documentClient, configuration);
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