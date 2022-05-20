using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Schedule.Actions;

public static class Get
{
    public record Query(DateTimeOffset Date) : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Title,
        int DurationMinutes,
        string[] Genres,
        double AverageScore,
        string Description,
        string ImageUrl,
        IReadOnlyList<ScreeningResult> Screenings);

    [PublicAPI]
    public record ScreeningResult(
        int Id,
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
            var start = request.Date.ToUniversalTime();
            var end = start.AddDays(1);

            var results = await _ctx.Films
                .Include(x => x.Reviews)
                .Include(x => x.Screenings)
                .Where(x => x.Screenings.Any(s => s.StartAt >= start && s.StartAt < end))
                .Select(x => new Result(x.Id,
                    x.Title,
                    (int) x.Duration.TotalMinutes,
                    x.Genres,
                    Math.Round(x.Reviews.Select(r => r.Score).DefaultIfEmpty().Average(), 1),
                    x.Description,
                    x.ImageUrl,
                    x.Screenings
                        .Where(s => s.StartAt >= start)
                        .Where(s => s.StartAt < end)
                        .Select(s => new ScreeningResult(
                            s.Id,
                            s.HallId,
                            s.StartAt,
                            s.BasePrice))
                        .ToList()))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
