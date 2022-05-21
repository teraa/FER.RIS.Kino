using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Screenings.Actions;

public static class Get
{
    public record Query(int Id) : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        int FilmId,
        string FilmTitle,
        int HallId,
        string HallName,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        decimal BasePrice,
        IReadOnlyList<SeatsRowResult> SeatRows);

    [PublicAPI]
    public record SeatsRowResult(
        int Row,
        IReadOnlyList<SeatResult> Seats);

    [PublicAPI]
    public record SeatResult(
        int Id,
        int Number,
        string Type,
        decimal PriceCoefficient,
        bool IsAvailable);

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
            var result = await _ctx.Screenings
                .Where(x => x.Id == request.Id)
                .Select(x => new Result(
                    x.Id,
                    x.FilmId,
                    x.Film.Title,
                    x.HallId,
                    x.Hall.Name,
                    x.StartAt,
                    x.StartAt + x.Film.Duration,
                    x.BasePrice,
                    x.Hall.Seats
                        .GroupBy(s => s.Row)
                        .Select(grouping => new SeatsRowResult(
                            grouping.Key,
                            grouping
                                .OrderBy(s => s.Number)
                                .Select(s => new SeatResult(
                                    s.Id,
                                    s.Number,
                                    s.Type,
                                    s.PriceCoefficient,
                                    s.Tickets.All(t => t.ScreeningId != request.Id)))
                                .ToList())
                        )
                        .ToList()))
                .FirstOrDefaultAsync(cancellationToken);

            if (result is null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }
    }
}
