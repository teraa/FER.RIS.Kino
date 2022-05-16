using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Films;

public static class Get
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Title,
        int DurationMinutes,
        string[] Genres);

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
                .Select(x => new Result(x.Id, x.Title, (int)x.Duration.TotalMinutes, x.Genres))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
