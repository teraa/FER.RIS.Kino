using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Films.Actions;

public static class Index
{
    public record Query(DateTimeOffset? ScreeningDate) : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Title,
        int DurationMinutes,
        string[] Genres,
        string Description,
        string ImageUrl,
        IReadOnlyList<ScreeningResult>? Screenings);

    [PublicAPI]
    public record ScreeningResult(
        int Id,
        int HallId,
        string HallName,
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

            var query = _ctx.Films
                .AsNoTracking()
                .OrderBy(x => x.Id);

            IQueryable<Result> resultsQuery;

            if (request.ScreeningDate.HasValue)
            {
                var start = request.ScreeningDate.Value.ToUniversalTime();
                var end = start.AddDays(1);

                resultsQuery = query
                    .Where(x => x.Screenings.Any(s => s.StartAt >= start && s.StartAt < end))
                    .Select(x => new Result(x.Id,
                        x.Title,
                        (int) x.Duration.TotalMinutes,
                        x.Genres,
                        x.Description,
                        x.ImageUrl,
                        x.Screenings
                            .Where(s => s.StartAt >= start)
                            .Where(s => s.StartAt < end)
                            .Select(s => new ScreeningResult(
                                s.Id,
                                s.HallId,
                                s.Hall.Name,
                                s.StartAt,
                                s.BasePrice))
                            .ToList()));
            }
            else
            {
                resultsQuery = query.Select(x => new Result(x.Id,
                    x.Title,
                    (int) x.Duration.TotalMinutes,
                    x.Genres,
                    x.Description,
                    x.ImageUrl,
                    null));
            }

            var results = await resultsQuery.ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
