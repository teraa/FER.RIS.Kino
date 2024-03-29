﻿using Kino.Features.Screenings.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Index = Kino.Features.Screenings.Actions.Index;

namespace Kino.Features.Screenings;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(AppPolicy.Admin)]
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
    /// <param name="date">Date to filter by (optional)</param>
    /// <param name="hallId">Hall ID to filter by (optional)</param>
    [HttpGet]
    [ProducesResponseType(typeof(Index.Result), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Index(DateTimeOffset? date, int? hallId, CancellationToken cancellationToken)
        => await _mediator.Send(new Index.Query(date, hallId), cancellationToken);

    /// <summary>
    /// Create Screening
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Create.Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
    /// <param name="id">Screening ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Delete.Command(id), cancellationToken);

    /// <summary>
    /// Get Screening
    /// </summary>
    /// <param name="id">Screening ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Get.Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Get.Query(id), cancellationToken);
}
