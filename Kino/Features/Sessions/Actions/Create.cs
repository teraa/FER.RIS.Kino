using FluentValidation;
using JetBrains.Annotations;
using Kino.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Sessions.Actions;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        string Name,
        string Password);

    [UsedImplicitly]
    public class ModelValidator : AbstractValidator<Model>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    [PublicAPI]
    public record Result(
        int Id,
        string Token,
        bool IsAdmin);

    [UsedImplicitly]
    public class Handler : IRequestHandler<Command, IActionResult>
    {
        private readonly KinoDbContext _ctx;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;

        public Handler(
            KinoDbContext ctx,
            PasswordService passwordService,
            TokenService tokenService)
        {
            _ctx = ctx;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var normalizedUsername = request.Model.Name.ToLowerInvariant();

            var entity = await _ctx.Users
                .AsNoTracking()
                .Include(x => x.Claims)
                .Where(x => x.Name == normalizedUsername)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity is null || !_passwordService.Test(request.Model.Password, entity.PasswordHash, entity.PasswordSalt))
                return new BadRequestResult();

            bool isAdmin = entity.Claims.Any(x => x.Type == AppClaim.Admin);

            var token = _tokenService.CreateToken(entity.Id, isAdmin);

            var result = new Result(entity.Id, token, isAdmin);

            return new OkObjectResult(result);
        }
    }
}
