using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Controllers.Tickets;

public static class Edit
{
    public record Command(
        int Id,
        Create.Model Model
    ) : IRequest<IActionResult>;

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
            var entity = await _ctx.Tickets
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
                return new NotFoundResult();

            entity.SeatId = request.Model.SeatId;
            entity.ScreeningId = request.Model.ScreeningId;

            try
            {
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return new BadRequestResult();
            }

            return new NoContentResult();
        }
    }
}
