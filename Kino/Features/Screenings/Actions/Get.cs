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
        int HallId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        decimal BasePrice,
        IReadOnlyList<SeatResult> Seats);

    [PublicAPI]
    public record SeatResult(
        int Id,
        int Number,
        int Row,
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
                    x.HallId,
                    x.StartAt,
                    x.StartAt + x.Film.Duration,
                    x.BasePrice,
                    x.Hall.Seats.Select(s => new SeatResult(
                        s.Id,
                        s.Number,
                        s.Row,
                        s.Type,
                        s.PriceCoefficient,
                        s.Tickets.All(t => t.ScreeningId != request.Id)))
                        .ToList()))
                .FirstOrDefaultAsync(cancellationToken);

            if (result is null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }
    }
}
