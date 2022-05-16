using Kino.Features.Screenings;
using Kino.Features.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#pragma warning disable CS8618

namespace Kino.Features.Tickets
{
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
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Ticket> Tickets { get; init; }
    }
}
