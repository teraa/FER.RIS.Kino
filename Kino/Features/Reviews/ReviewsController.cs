using Kino.Features.Reviews.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Index = Kino.Features.Reviews.Actions.Index;

namespace Kino.Features.Reviews;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get All Reviews
    /// </summary>
    /// <param name="userId">User ID to filter by (optional)</param>
    [HttpGet]
    [ProducesResponseType(typeof(Index.Result), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int? userId, CancellationToken cancellationToken)
        => await _mediator.Send(new Index.Query(userId), cancellationToken);

    /// <summary>
    /// Create Review
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Create.Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Create.Command(HttpContext.GetUserId(), model), cancellationToken);

    /// <summary>
    /// Edit Review
    /// </summary>
    /// <param name="id">Review ID</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(int id, Edit.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Edit.Command(id, model), cancellationToken);

    /// <summary>
    /// Delete Review
    /// </summary>
    /// <param name="id">Review ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Delete.Command(id), cancellationToken);
}
