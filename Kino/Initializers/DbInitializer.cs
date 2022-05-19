using Extensions.Hosting.AsyncInitialization;
using JetBrains.Annotations;
using Kino.Features;
using Kino.Features.Films;
using Kino.Features.Halls;
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
        const string password = "admin";

        bool exists = await _ctx.Users
            .Where(x => x.Name == username)
            .AnyAsync();

        if (exists)
            return;

        var (hash, salt) = _passwordService.Hash(password);

        var user = new User
        {
            Name = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            Claims = new List<Claim>
            {
                new()
                {
                    Type = AppClaim.Admin,
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
                Description = "The Batman is a 2022 American superhero film based on the DC Comics character Batman.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/f/ff/The_Batman_%28film%29_poster.jpg/220px-The_Batman_%28film%29_poster.jpg",
                Screenings = new List<Screening>
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
                Description = "Sonic the Hedgehog 2[b] is a 2022 action-adventure comedy film based on the video game franchise published by Sega, and the sequel to Sonic the Hedgehog (2020).",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/3e/Sonic_the_Hedgehog_2_film_poster.jpg",
                Screenings = new List<Screening> { },
            },
            new()
            {
                Title = "Fantastic Beasts: The Secrets of Dumbledore",
                Duration = new TimeSpan(2, 22, 0),
                Genres = new[] {"akcija", "avantura"},
                Description = "Fantastic Beasts: The Secrets of Dumbledore is a 2022 fantasy film directed by David Yates from a screenplay by J. K. Rowling and Steve Kloves.",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/3/34/Fantastic_Beasts-_The_Secrets_of_Dumbledore.png/220px-Fantastic_Beasts-_The_Secrets_of_Dumbledore.png",
                Screenings = new List<Screening> { },
            },
        };

        _ctx.Films.AddRange(films);


        await _ctx.SaveChangesAsync();
    }
}
