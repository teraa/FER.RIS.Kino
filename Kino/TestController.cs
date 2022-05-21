using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kino;

// TODO: Remove
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok("Hello World!");

    [HttpGet("Auth")]
    [Authorize]
    public IActionResult GetAuthenticated()
        => Ok("Authenticated");

    [HttpGet("Admin")]
    [Authorize(AppPolicy.Admin)]
    public IActionResult GetAdmin()
        => Ok("Admin");
}
