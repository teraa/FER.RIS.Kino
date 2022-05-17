using Kino.Features.Screenings.Actions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Features.Screenings;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ScreeningsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScreeningsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get All Screenings
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Get.Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => await _mediator.Send(new Get.Query(), cancellationToken);

    /// <summary>
    /// Create Screening
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Create.Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Create.Command(model), cancellationToken);

    /// <summary>
    /// Edit Screening
    /// </summary>
    /// <param name="id">Screening ID</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(int id, Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Edit.Command(id, model), cancellationToken);

    /// <summary>
    /// Delete Screening
    /// </summary>
    /// <param name="id">Screening</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Delete.Command(id), cancellationToken);
}
