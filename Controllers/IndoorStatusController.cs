using Microsoft.AspNetCore.Mvc;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndoorStatusController : ControllerBase
{
    private IndoorStatusManager indoorStatusManager;
    private ILogger logger;
    public IndoorStatusController( IConfiguration configuration, ILogger log)
    {
        logger = log;
        indoorStatusManager = new IndoorStatusManager(configuration, logger);
    }

    [HttpGet]
    public async Task<ActionResult<IndoorStatusData>> Get()
    {
        var data = await indoorStatusManager.GetIndoorStatus();
        
        return Ok(data);

    }

    [HttpPost]
    public async Task<ActionResult<IndoorStatusData>> Post([FromBody]string status)
    {
        Console.WriteLine("####################################################");
        Console.WriteLine("Status: " + status);
        Console.WriteLine("####################################################");
        var data = await indoorStatusManager.SaveIndoorStatus(status);

        //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        return Ok();
    }
}