using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kino;

#if DEBUG
[ApiController]
[Route("[controller]")]
public class DebugController : ControllerBase
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
#endif
