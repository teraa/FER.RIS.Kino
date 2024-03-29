﻿using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Reviews.Actions;

public static class Create
{
    public record Command(
        int UserId,
        Model Model
    ) : IRequest<IActionResult>;

    public record Model(
        int FilmId,
        int Score,
        string Text);

    [UsedImplicitly]
    public class ModelValidator : AbstractValidator<Model>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Score).InclusiveBetween(1, 10);
            RuleFor(x => x.Text).NotEmpty();
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
            var entity = new Review
            {
                UserId = request.UserId,
                FilmId = request.Model.FilmId,
                Score = request.Model.Score,
                Text = request.Model.Text,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            _ctx.Reviews.Add(entity);

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
