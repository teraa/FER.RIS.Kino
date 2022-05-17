using Kino.Features.Sessions.Actions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Features.Sessions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class SessionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create Session (Login)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Create.Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Create.Command(model), cancellationToken);
}
