using JetBrains.Annotations;
using Kino.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Users.Actions;

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
        private readonly PasswordService _passwordService;

        public Handler(KinoDbContext ctx, PasswordService passwordService)
        {
            _ctx = ctx;
            _passwordService = passwordService;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.Users
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
                return new NotFoundResult();


            var password = _passwordService.Hash(request.Model.Password);

            entity.Name = request.Model.Name;
            entity.PasswordHash = password.hash;
            entity.PasswordSalt = password.salt;

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
