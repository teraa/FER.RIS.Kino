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
        await _ctx.Database.MigrateAsync();
    }
}
