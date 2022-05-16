using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Tickets;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        int SeatId,
        int ScreeningId);

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
            var entity = new Ticket
            {
                SeatId = request.Model.SeatId,
                ScreeningId = request.Model.ScreeningId,
            };

            _ctx.Tickets.Add(entity);

            try
            {
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return new BadRequestResult();
            }

            var result = new Result(entity.Id);

            return new OkObjectResult(result);
        }
    }
}
