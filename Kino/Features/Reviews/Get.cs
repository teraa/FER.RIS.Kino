using JetBrains.Annotations;
using Kino.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Reviews;

public static class Get
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        int UserId,
        int FilmId,
        int Score,
        string Text,
        DateTimeOffset CreatedAt);

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
            var results = await _ctx.Reviews
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id, x.UserId, x.FilmId, x.Score, x.Text, x.CreatedAt))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
