using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Screenings.Actions;

public static class Index
{
    public record Query(DateTimeOffset? Date) : IRequest<IActionResult>;

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
            var query = _ctx.Screenings
                .AsNoTracking();

            if (request.Date is { } date)
            {
                query = query
                    .Where(x => x.StartAt.Day == date.Day)
                    .Where(x => x.StartAt.Month == date.Month)
                    .Where(x => x.StartAt.Year == date.Year);
            }

            var results = await query
                .OrderBy(x => x.StartAt)
                .Select(x => new Result(x.Id, x.FilmId, x.HallId, x.StartAt, x.BasePrice))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
