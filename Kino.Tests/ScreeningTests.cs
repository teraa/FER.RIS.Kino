using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kino.Features.Films;
using Kino.Features.Halls;
using Kino.Features.Screenings;
using Kino.Features.Screenings.Actions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Kino.Tests;

[Collection("Integration")]
public class ScreeningTests : IClassFixture<AppFactory>, IAsyncLifetime
{
    private readonly AppFactory _factory;
    private IServiceScope _scope = null!;
    private KinoDbContext _ctx = null!;
    private IMediator _mediator = null!;

    public ScreeningTests(AppFactory factory)
    {
        _factory = factory;
    }
    
    public async Task InitializeAsync()
    {
        _scope = _factory.Services.CreateScope();
        _ctx = _scope.ServiceProvider.GetRequiredService<KinoDbContext>();
        await _ctx.Database.BeginTransactionAsync();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    public async Task DisposeAsync()
    {
        await _ctx.Database.RollbackTransactionAsync();
        _scope.Dispose();
    }

    [Fact]
    public async Task Add_ConflictingTimesScreenings_Error()
    {
        var startAt = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
        
        var hall = new Hall
        {
            Name = "",
            Capacity = 0,
        };

        var screening = new Screening()
        {
            StartAt = startAt,
            BasePrice = 0,
            Hall = hall,
        };
        
        var film = new Film
        {
            Title = "",
            Duration = TimeSpan.FromMinutes(5),
            Genres = Array.Empty<string>(),
            Description = "",
            ImageUrl = "",
            Screenings = new List<Screening>
            {
                screening,
            },
        };
        _ctx.Films.Add(film);
        await _ctx.SaveChangesAsync();
        
        var result = await _mediator.Send(new Create.Command(new Create.Model(
            FilmId: film.Id,
            HallId: hall.Id,
            StartAt: startAt + film.Duration,
            BasePrice: 0)), default);

        Assert.IsType<ConflictResult>(result);
    }
    
    [Fact]
    public async Task Add_NonConflictingScreenings_Ok()
    {
        var startAt = new DateTimeOffset(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
        
        var hall = new Hall
        {
            Name = "",
            Capacity = 0,
        };

        var screening = new Screening()
        {
            StartAt = startAt,
            BasePrice = 0,
            Hall = hall,
        };
        
        var film = new Film
        {
            Title = "",
            Duration = TimeSpan.FromMinutes(5),
            Genres = Array.Empty<string>(),
            Description = "",
            ImageUrl = "",
            Screenings = new List<Screening>
            {
                screening,
            },
        };
        _ctx.Films.Add(film);
        await _ctx.SaveChangesAsync();
        
        var result = await _mediator.Send(new Create.Command(new Create.Model(
            FilmId: film.Id,
            HallId: hall.Id,
            StartAt: startAt + film.Duration + TimeSpan.FromMinutes(1),
            BasePrice: 0)), default);

        Assert.IsType<OkObjectResult>(result);
    }
}