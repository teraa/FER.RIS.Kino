using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Screenings;

public static class Get
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        int FilmId,
        int HallId,
        DateTimeOffset StartAt,
        decimal BasePrice);

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
            var results = await _ctx.Screenings
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id, x.FilmId, x.HallId, x.StartAt, x.BasePrice))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
