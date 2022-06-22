using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using KioskApi.Models;
using KioskApi.Managers;

namespace KioskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClothingController : ControllerBase
{
    ClothingManager clothingManager;
    public ClothingController(IDocumentClient documentClient, IConfiguration configuration){
        clothingManager = new ClothingManager(documentClient, configuration);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BodyPart>>> GetAllClothes(){

        var data = await clothingManager.GetClothing();
        return Ok(data);
        
    }

    [HttpGet]
    [Route("BodyParts")]
    public async Task<ActionResult<IEnumerable<BodyPart>>> GetBodyParts(){

        var data = await clothingManager.GetBodyParts();
        return Ok(data);
        
    }

    [HttpGet]
    [Route("Calculate")]
    public async Task<IEnumerable<PersonsClothing>> GetClothingCalculated(
        [FromQuery] string feels, 
        [FromQuery] string ids, 
        [FromQuery] string names, 
        [FromQuery] string colors, 
        [FromQuery] string lat, 
        [FromQuery] string lon){

        var data = await clothingManager.GetCalcuatedClothing(feels, ids, names, colors,lat, lon);
        return data;
        
    }

    
}