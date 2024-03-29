﻿using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Films.Actions;

public static class Delete
{
    public record Command(
        int Id
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

            _ctx.Films.Remove(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}
