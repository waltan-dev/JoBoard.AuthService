using Microsoft.AspNetCore.Mvc;

namespace JoBoard.AuthService.Controllers;

[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}