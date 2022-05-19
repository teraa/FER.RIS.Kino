using Kino.Features;
using Kino.Features.Halls;
using Kino.Features.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#pragma warning disable CS8618

namespace Kino.Features
{
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
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Seat> Seats { get; init; }
    }
}
