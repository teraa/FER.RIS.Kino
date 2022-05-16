using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Reviews;

public static class Edit
{
    public record Command(
        int Id,
        Model Model
    ) : IRequest<IActionResult>;

    public record Model(
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
            var entity = await _ctx.Reviews
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
                return new NotFoundResult();

            entity.Score = request.Model.Score;
            entity.Text = request.Model.Text;

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
