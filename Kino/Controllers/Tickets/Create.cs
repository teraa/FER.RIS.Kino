using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Controllers.Tickets;

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
            await _ctx.SaveChangesAsync(cancellationToken);

            var result = new Result(entity.Id);

            return new CreatedAtActionResult(
                actionName: nameof(Get),
                controllerName: nameof(TicketsController),
                routeValues: result,
                value: result);
        }
    }
}
