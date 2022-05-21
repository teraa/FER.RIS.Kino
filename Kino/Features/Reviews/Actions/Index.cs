using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Reviews.Actions;

public static class Index
{
    public record Query(int? UserId) : IRequest<IActionResult>;

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
            var query = _ctx.Reviews
                .AsNoTracking();

            if (request.UserId.HasValue)
                query = query.Where(x => x.UserId == request.UserId);
            
            var results = await query
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id, x.UserId, x.FilmId, x.Score, x.Text, x.CreatedAt))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
