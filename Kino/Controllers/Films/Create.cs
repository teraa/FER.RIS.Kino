using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kino.Controllers.Films;

public static class Create
{
    public record Command(
        string Title,
        int DurationMinutes,
        string[] Genres
    ) : IRequest<IActionResult>;

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
                Title = request.Title,
                Duration = TimeSpan.FromMinutes(request.DurationMinutes),
                Genres = request.Genres,
            };

            _ctx.Films.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            var result = new Result(entity.Id);

            return new CreatedAtActionResult(
                actionName: nameof(Get),
                controllerName: nameof(Films),
                routeValues: result,
                value: result);
        }
    }
}
