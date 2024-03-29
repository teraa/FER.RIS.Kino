﻿using Kino.Features.Tickets.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Index = Kino.Features.Tickets.Actions.Index;

namespace Kino.Features.Tickets;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get All Tickets
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Index.Result), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
        => await _mediator.Send(new Index.Query(), cancellationToken);

    /// <summary>
    /// Create Ticket
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Create.Result), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Create(Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Create.Command(model), cancellationToken);

    /// <summary>
    /// Edit Ticket
    /// </summary>
    /// <param name="id">Ticket ID</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(int id, Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Edit.Command(id, model), cancellationToken);

    /// <summary>
    /// Delete Ticket
    /// </summary>
    /// <param name="id">Ticket ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Delete.Command(id), cancellationToken);
}
