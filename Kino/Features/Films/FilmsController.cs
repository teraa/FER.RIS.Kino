﻿using Kino.Features.Films.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Index = Kino.Features.Films.Actions.Index;

namespace Kino.Features.Films;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(AppPolicy.Admin)]
public class FilmsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilmsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get All Films
    /// </summary>
    /// <param name="screeningDate">Filter by screening date (optional)</param>
    [HttpGet]
    [ProducesResponseType(typeof(Index.Result), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Index(DateTimeOffset? screeningDate, CancellationToken cancellationToken)
        => await _mediator.Send(new Index.Query(screeningDate), cancellationToken);

    /// <summary>
    /// Create Film
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Create.Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Create.Command(model), cancellationToken);

    /// <summary>
    /// Edit Film
    /// </summary>
    /// <param name="id">Film ID</param>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(int id, Create.Model model, CancellationToken cancellationToken)
        => await _mediator.Send(new Edit.Command(id, model), cancellationToken);

    /// <summary>
    /// Delete Film
    /// </summary>
    /// <param name="id">Film ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Delete.Command(id), cancellationToken);

    /// <summary>
    /// Get Film
    /// </summary>
    /// <param name="id">Film ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Get.Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        => await _mediator.Send(new Get.Query(id), cancellationToken);
}
