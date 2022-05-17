using FluentValidation;
using JetBrains.Annotations;
using Kino.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Users.Actions;

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
        string Token);

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

            bool exists = await _ctx.Users
                .Where(x => x.Name == normalizedUsername)
                .AnyAsync(cancellationToken);

            if (exists)
                return new ConflictResult();

            var password = _passwordService.Hash(request.Model.Password);

            var entity = new User
            {
                Name = normalizedUsername,
                PasswordHash = password.hash,
                PasswordSalt = password.salt,
            };

            _ctx.Users.Add(entity);

            try
            {
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return new BadRequestResult();
            }

            var token = _tokenService.CreateToken(entity.Id);

            var result = new Result(entity.Id, token);

            return new OkObjectResult(result);
        }
    }
}
