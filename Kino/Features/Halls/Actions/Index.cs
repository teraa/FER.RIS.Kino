using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Halls.Actions;

public static class Index
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Name,
        int Capacity);

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
            var results = await _ctx.Halls
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id, x.Name, x.Capacity))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
