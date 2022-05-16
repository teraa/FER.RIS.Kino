using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Controllers.Films;

public static class Edit
{
    public record Command(
        int Id,
        Model Model
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
            var entity = await _ctx.Films
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
                return new NotFoundResult();

            entity.Title = request.Model.Title;
            entity.Duration = TimeSpan.FromMinutes(request.Model.DurationMinutes);
            entity.Genres = request.Model.Genres;

            await _ctx.SaveChangesAsync(cancellationToken);
            return new NoContentResult();
        }
    }
}
