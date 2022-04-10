using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global

#pragma warning disable CS8618
namespace Kino.Data;

public class KinoDbContext : DbContext
{
    public KinoDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Hall> Halls { get; init; }
    public DbSet<Seat> Seats { get; init; }
    public DbSet<Ticket> Tickets { get; init; }
    public DbSet<Screening> Screenings { get; init; }
    public DbSet<Film> Films { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Claim> Claims { get; init; }
    public DbSet<Review> Reviews { get; init; }

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
#pragma warning disable CS8618

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    
    public ICollection<Seat> Seats { get; set; }
    public ICollection<Screening> Screenings { get; set; }
}

public class Seat
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Row { get; set; }
    public int HallId { get; set; }
    public string Type { get; set; }
    public decimal PriceCoefficient { get; set; }
    
    public Hall Hall { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}

public class SeatConfig : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasIndex(x => new {x.Number, x.Row, x.HallId})
            .IsUnique();
    }
}

public class Ticket
{
    public int Id { get; set; }
    public int SeatId { get; set; }
    public int ScreeningId { get; set; }
    
    public Seat Seat { get; set; }
    public Screening Screening { get; set; }
}

public class TicketConfig : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasIndex(x => new {x.SeatId, x.ScreeningId})
            .IsUnique();
    }
}

public class Screening
{
    public int Id { get; set; }
    public int FilmId { get; set; }
    public int HallId { get; set; }
    public DateTimeOffset StartAt { get; set; }
    public decimal BasePrice { get; set; }
    
    public Film Film { get; set; }
    public Hall Hall { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string[] Genres { get; set; }
    
    public ICollection<Screening> Screenings { get; set; }
    public ICollection<Review> Reviews { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public ICollection<Claim> Claims { get; set; }
    public ICollection<Review> Reviews { get; set; }
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}

public class Claim
{
    public const string Admin = nameof(Admin);
    
    public int UserId { get; set; }
    public string Type { get; set; }
    
    public User User { get; set; }
}

public class ClaimConfig : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.HasKey(x => new {x.UserId, x.Type});
    }
}

public class Review
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FilmId { get; set; }
    public int Score { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public User User { get; set; }
    public Film Film { get; set; }
}