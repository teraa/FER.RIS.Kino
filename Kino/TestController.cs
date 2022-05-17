using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kino;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }

    [HttpGet("Auth")]
    [Authorize]
    public IActionResult GetAuthenticated()
    {
        return Ok("Authenticated");
    }
}
