using Extensions.Hosting.AsyncInitialization;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Kino.Initializers;

[UsedImplicitly]
public class MigrationInitializer : IAsyncInitializer
{
    private readonly KinoDbContext _ctx;

    public MigrationInitializer(KinoDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task InitializeAsync()
    {
        var migrations = await _ctx.Database.GetPendingMigrationsAsync();

        await _ctx.Database.MigrateAsync();

        if (migrations.Contains("20220519152334_Add_Film_DescriptionAndImageUrl"))
        {
            var film = await _ctx.Films.FirstOrDefaultAsync(x => x.Title == "The Batman");
            if (film is { })
            {
                film.Description = "The Batman is a 2022 American superhero film based on the DC Comics character Batman.";
                film.ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/f/ff/The_Batman_%28film%29_poster.jpg/220px-The_Batman_%28film%29_poster.jpg";
            }

            film = await _ctx.Films.FirstOrDefaultAsync(x => x.Title == "Sonic the Hedgehog 2");
            if (film is { })
            {
                film.Description = "Sonic the Hedgehog 2[b] is a 2022 action-adventure comedy film based on the video game franchise published by Sega, and the sequel to Sonic the Hedgehog (2020).";
                film.ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/3e/Sonic_the_Hedgehog_2_film_poster.jpg";
            }

            film = await _ctx.Films.FirstOrDefaultAsync(x => x.Title == "Fantastic Beasts: The Secrets of Dumbledore");
            if (film is { })
            {
                film.Description = "Fantastic Beasts: The Secrets of Dumbledore is a 2022 fantasy film directed by David Yates from a screenplay by J. K. Rowling and Steve Kloves.";
                film.ImageUrl = "https://upload.wikimedia.org/wikipedia/en/thumb/3/34/Fantastic_Beasts-_The_Secrets_of_Dumbledore.png/220px-Fantastic_Beasts-_The_Secrets_of_Dumbledore.png";
            }

            await _ctx.SaveChangesAsync();
        }
    }
}
