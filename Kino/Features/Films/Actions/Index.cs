using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Films.Actions;

public static class Index
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Title,
        int DurationMinutes,
        string[] Genres,
        string Description,
        string ImageUrl);

    [UsedImplicitly]
    public class Handler : IRequestHandler<Query, IActionResult>
    {
        private readonly KinoDbContext _ctx;

        public Handler(KinoDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var results = await _ctx.Films
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id,
                    x.Title,
                    (int) x.Duration.TotalMinutes,
                    x.Genres,
                    x.Description,
                    x.ImageUrl))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
