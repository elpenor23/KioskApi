using Microsoft.AspNetCore.Mvc;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoonPhaseController : ControllerBase
{
    private MoonPhaseManager moonPhaseManager;
    private ILogger logger;
    public MoonPhaseController(IConfiguration configuration, ILogger log)
    {
        logger = log;
        moonPhaseManager = new MoonPhaseManager(configuration, logger);
    }

    [HttpGet]
    public async Task<ActionResult<MoonData>> GetMoonPhase([FromQuery] decimal? lat, [FromQuery] decimal? lon)
    {
        if (lat == null || lon == null) {return NotFound();}

        var data = await moonPhaseManager.GetMoonPhase(lat.Value, lon.Value);
        
        return Ok(data);

    }
}