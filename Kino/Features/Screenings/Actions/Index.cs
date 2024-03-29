﻿using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Screenings.Actions;

public static class Index
{
    public record Query(
        DateTimeOffset? Date,
        int? HallId
    ) : IRequest<IActionResult>;

    [PublicAPI]
    public record Result(
        int Id,
        int FilmId,
        int HallId,
        DateTimeOffset StartAt,
        DateTimeOffset EndAt,
        decimal BasePrice);

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
            var query = _ctx.Screenings
                .AsNoTracking();

            if (request.Date.HasValue)
            {
                var start = request.Date.Value.ToUniversalTime();
                var end = start.AddDays(1);

                query = query
                    .Where(x => x.StartAt >= start)
                    .Where(x => x.StartAt < end);
            }

            if (request.HallId.HasValue)
            {
                query = query
                    .Where(x => x.HallId == request.HallId);
            }

            var results = await query
                .OrderBy(x => x.StartAt)
                .Select(x => new Result(x.Id,
                    x.FilmId,
                    x.HallId,
                    x.StartAt,
                    x.StartAt + x.Film.Duration,
                    x.BasePrice))
                .ToListAsync(cancellationToken);

            return new OkObjectResult(results);
        }
    }
}
