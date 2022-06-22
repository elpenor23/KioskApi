using Microsoft.AspNetCore.Mvc;

namespace KioskApi.Controllers;

[Route("error/[controller]")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{

    [Route("/error")]
    public IActionResult HandleError() => Problem();
}