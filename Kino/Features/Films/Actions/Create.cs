﻿using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kino.Features.Films.Actions;

public static class Create
{
    public record Command(Model Model)
        : IRequest<IActionResult>;

    public record Model(
        string Title,
        int DurationMinutes,
        string[] Genres,
        string Description,
        string ImageUrl);

    [UsedImplicitly]
    public class ModelValidator : AbstractValidator<Model>
    {
        public ModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.DurationMinutes).GreaterThan(0);
            RuleForEach(x => x.Genres).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ImageUrl).NotEmpty();
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
            var entity = new Film
            {
                Title = request.Model.Title,
                Duration = TimeSpan.FromMinutes(request.Model.DurationMinutes),
                Genres = request.Model.Genres,
                Description = request.Model.Description,
                ImageUrl = request.Model.ImageUrl,
            };

            _ctx.Films.Add(entity);

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
