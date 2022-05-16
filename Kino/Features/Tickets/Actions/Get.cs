using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Tickets.Actions;

public static class Get
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        int SeatId,
        int ScreeningId);

    [UsedImplicitly]
    public class Handler : IRequestHandler<Query, IActionResult>
    {
        private readonly KinoDbContext _ctx;

        public Handler(KinoDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var results = await _ctx.Tickets
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id, x.SeatId, x.ScreeningId))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
