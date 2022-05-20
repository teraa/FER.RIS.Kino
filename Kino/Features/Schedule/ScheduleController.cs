using Kino.Features.Schedule.Actions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Features.Schedule;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[AllowAnonymous]
public class ScheduleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get Schedule
    /// </summary>
    /// <param name="date">Date to filter by</param>
    [HttpGet]
    [ProducesResponseType(typeof(Get.Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSchedule(DateTimeOffset date, CancellationToken cancellationToken)
        => await _mediator.Send(new Get.Query(date), cancellationToken);
}
