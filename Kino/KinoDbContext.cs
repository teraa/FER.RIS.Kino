using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace Kino;

public partial class KinoDbContext : DbContext
{
    public KinoDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KinoDbContext).Assembly);
    }
}
