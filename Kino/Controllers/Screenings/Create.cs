using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Controllers.Screenings;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        int FilmId,
        int HallId,
        DateTimeOffset StartAt,
        decimal BasePrice);

    [PublicAPI]
    public record Result(int Id);

    [UsedImplicitly]
    public class Handler : IRequestHandler<Command, IActionResult>
    {
        private readonly KinoDbContext _ctx;

        public Handler(KinoDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = new Screening
            {
                FilmId = request.Model.FilmId,
                HallId = request.Model.HallId,
                StartAt = request.Model.StartAt,
                BasePrice = request.Model.BasePrice,
            };

            _ctx.Screenings.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            var result = new Result(entity.Id);

            return new OkObjectResult(result);
        }
    }
}
