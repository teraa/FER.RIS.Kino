using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Index = Kino.Features.Halls.Actions.Index;

namespace Kino.Features.Halls;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize]
public class HallsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HallsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get All Halls
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Index.Result), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
        => await _mediator.Send(new Index.Query(), cancellationToken);
}
