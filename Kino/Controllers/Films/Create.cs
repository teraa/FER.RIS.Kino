using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Controllers.Films;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        string Title,
        int DurationMinutes,
        string[] Genres);

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
            var entity = new Film
            {
                Title = request.Model.Title,
                Duration = TimeSpan.FromMinutes(request.Model.DurationMinutes),
                Genres = request.Model.Genres,
            };

            _ctx.Films.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            var result = new Result(entity.Id);

            return new CreatedAtActionResult(
                actionName: nameof(Get),
                controllerName: nameof(FilmsController),
                routeValues: result,
                value: result);
        }
    }
}
