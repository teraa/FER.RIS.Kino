using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Users.Actions;

public static class Index
{
    public record Query : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        string Name);

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
            var results = await _ctx.Users
                .OrderBy(x => x.Id)
                .Select(x => new Result(x.Id, x.Name))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
