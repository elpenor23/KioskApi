using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndoorStatusController : ControllerBase
{
    IndoorStatusManager indoorStatusManager;
    public IndoorStatusController(IDocumentClient documentClient, IConfiguration configuration)
    {
        indoorStatusManager = new IndoorStatusManager(documentClient, configuration);
    }

    [HttpGet]
    public async Task<ActionResult<IndoorStatusData>> Get()
    {
        var data = await indoorStatusManager.GetIndoorStatus();
        
        return Ok(data);

    }

    [HttpPost]
    // TODO: Figure out how to get this from the posted body
    public async Task<ActionResult<IndoorStatusData>> PostTodoItem([FromBody]string thing_data)
    {
        Console.WriteLine("Status: " + thing_data);
        //var data = await indoorStatusManager.SaveIndoorStatus(status);

        //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        return Ok();
    }
}