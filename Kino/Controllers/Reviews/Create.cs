using FluentValidation;
using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Controllers.Reviews;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        int UserId, // TODO: from session
        int FilmId,
        int Score,
        string Text);

    [UsedImplicitly]
    public class ModelValidator : AbstractValidator<Model>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Score).InclusiveBetween(1, 10);
            RuleFor(x => x.Text).NotEmpty();
        }
    }

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
            var entity = new Review
            {
                UserId = request.Model.UserId,
                FilmId = request.Model.FilmId,
                Score = request.Model.Score,
                Text = request.Model.Text,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            _ctx.Reviews.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            var result = new Result(entity.Id);

            return new OkObjectResult(result);
        }
    }
}
