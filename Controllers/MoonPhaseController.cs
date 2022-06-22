using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoonPhaseController : ControllerBase
{
    MoonPhaseManager moonPhaseManager;
    public MoonPhaseController(IDocumentClient documentClient, IConfiguration configuration)
    {
        moonPhaseManager = new MoonPhaseManager(documentClient, configuration);
    }

    [HttpGet]
    public async Task<ActionResult<MoonData>> GetMoonPhase([FromQuery] decimal? lat, [FromQuery] decimal? lon)
    {
        if (lat == null || lon == null) {return NotFound();}

        var data = await moonPhaseManager.GetMoonPhase(lat.Value, lon.Value);
        
        return Ok(data);

    }
}