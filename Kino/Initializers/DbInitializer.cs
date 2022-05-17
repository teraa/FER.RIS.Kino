using Extensions.Hosting.AsyncInitialization;
using JetBrains.Annotations;
using Kino.Features;
using Kino.Features.Films;
using Kino.Features.Screenings;
using Kino.Features.Users;
using Kino.Services;
using Microsoft.EntityFrameworkCore;

namespace Kino.Initializers;

[UsedImplicitly]
public class DbInitializer : IAsyncInitializer
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly KinoDbContext _ctx;
    private readonly PasswordService _passwordService;

    public DbInitializer(
        IHostEnvironment hostEnvironment,
        KinoDbContext ctx,
        PasswordService passwordService)
    {
        _hostEnvironment = hostEnvironment;
        _ctx = ctx;
        _passwordService = passwordService;
    }

    public async Task InitializeAsync()
    {
        if (!_hostEnvironment.IsDevelopment())
            return;

        const string username = "admin";

        bool exists = await _ctx.Users
            .Where(x => x.Name == username)
            .AnyAsync();

        if (exists)
            return;

        var password = _passwordService.Hash(password: username);

        var user = new User
        {
            Name = username,
            PasswordHash = password.hash,
            PasswordSalt = password.salt,
            Claims = new List<Claim>
            {
                new()
                {
                    Type = "admin",
                }
            }
        };

        _ctx.Users.Add(user);

        var halls = new Hall[]
        {
            new()
            {
                Name = "Dvorana A",
                Capacity = 220,
                Seats = new List<Seat>(),
            },
            new()
            {
                Name = "Dvorana B",
                Capacity = 100,
                Seats = new List<Seat>(),
            },
        };

        _ctx.Halls.AddRange(halls);

        foreach (var hall in halls)
        {
            for (int i = 1; i <= hall.Capacity; i++)
            {
                const int perRow = 20;

                (string type, decimal priceCoefficient) = (100d * i / hall.Capacity) switch
                {
                    > 90 => ("Couple", 1.5m),
                    > 70 => ("Pro", 1.25m),
                    _ => ("Normal", 1m),
                };

                var seat = new Seat
                {
                    Number = i / perRow + 1,
                    Row = i % perRow + 1,
                    Type = type,
                    PriceCoefficient = priceCoefficient,
                };

                hall.Seats.Add(seat);
            }
        }

        var films = new Film[]
        {
            new()
            {
                Title = "The Batman",
                Duration = new TimeSpan(2, 56, 0),
                Genres = new[] {"akcija", "drama"},
                Screenings = new Screening[]
                {
                    new()
                    {
                        Hall = halls[0],
                        StartAt = new DateTimeOffset(2022, 6, 1, 20, 0, 0, TimeSpan.Zero),
                        BasePrice = 50,
                    }
                },
            },
            new()
            {
                Title = "Sonic the Hedgehog 2",
                Duration = new TimeSpan(2, 2, 0),
                Genres = new[] {"avantura", "obiteljski"},
                Screenings = new Screening[] { },
            },
            new()
            {
                Title = "Fantastic Beasts: The Secrets of Dumbledore",
                Duration = new TimeSpan(2, 22, 0),
                Genres = new[] {"akcija", "avantura"},
                Screenings = new Screening[] { },
            },
        };

        _ctx.Films.AddRange(films);


        await _ctx.SaveChangesAsync();
    }
}
