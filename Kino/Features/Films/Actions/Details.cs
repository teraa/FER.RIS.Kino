using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Films.Actions;

public static class Details
{
    public record Query(int Id) : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Title,
        int DurationMinutes,
        string[] Genres,
        double AverageScore,
        IReadOnlyList<ReviewResult> Reviews,
        IReadOnlyList<ScreeningResult> Screenings);

    [PublicAPI]
    public record ReviewResult(
        int Id,
        int UserId,
        string UserName,
        int Score,
        string Text,
        DateTimeOffset CreatedAt);

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
            var result = await _ctx.Films
                .Include(x => x.Reviews)
                .Include(x => x.Screenings)
                .Where(x => x.Id == request.Id)
                .Select(x => new Result(x.Id,
                    x.Title,
                    (int) x.Duration.TotalMinutes,
                    x.Genres,
                    x.Reviews.Select(r => r.Score).DefaultIfEmpty().Average(),
                    x.Reviews.Select(r => new ReviewResult(r.Id, r.UserId, r.User.Name, r.Score, r.Text, r.CreatedAt)).ToList(),
                    x.Screenings.Select(s => new ScreeningResult(s.Id, s.HallId, s.StartAt, s.BasePrice)).ToList()))
                .FirstOrDefaultAsync(cancellationToken);

            if (result is null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }
    }
}
