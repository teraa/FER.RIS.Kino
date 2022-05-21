using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Screenings.Actions;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        int FilmId,
        int HallId,
        DateTimeOffset StartAt,
        decimal BasePrice);

    [UsedImplicitly]
    public class ModelValidator : AbstractValidator<Model>
    {
        public ModelValidator()
        {
            RuleFor(x => x.StartAt).NotEmpty();
            RuleFor(x => x.BasePrice).GreaterThanOrEqualTo(0);
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
            var duration = await _ctx.Films
                .Where(x => x.Id == request.Model.FilmId)
                .Select(x => (TimeSpan?)x.Duration)
                .FirstOrDefaultAsync(cancellationToken);

            if (duration is null)
                return new BadRequestResult();

            var start = request.Model.StartAt.ToUniversalTime();
            var end = start + duration;

            bool overlaps = await _ctx.Screenings
                .Where(x => x.HallId == request.Model.HallId)
                .Where(x => x.StartAt <= end)
                .Where(x => start <= x.StartAt + x.Film.Duration)
                .AnyAsync(cancellationToken);

            if (overlaps)
                return new ConflictResult();

            var entity = new Screening
            {
                FilmId = request.Model.FilmId,
                HallId = request.Model.HallId,
                StartAt = request.Model.StartAt,
                BasePrice = request.Model.BasePrice,
            };

            _ctx.Screenings.Add(entity);

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
