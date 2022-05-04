using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kino.Data;

#pragma warning disable CS8618
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

public partial class KinoDbContext
{
    public DbSet<Ticket> Tickets { get; init; }
}
