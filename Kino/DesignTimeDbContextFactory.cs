using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kino;

[UsedImplicitly]
internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<KinoDbContext>
{
    public KinoDbContext CreateDbContext(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var dbOptions = builder.Configuration.GetOptions<DbOptions>();

        var optionsBuilder = new DbContextOptionsBuilder<KinoDbContext>()
            .UseNpgsql(dbOptions.ConnectionString);

        return new KinoDbContext(optionsBuilder.Options);
    }
}
