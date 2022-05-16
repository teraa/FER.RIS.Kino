using Kino.Features.Films;
using Kino.Features.Screenings;
using Kino.Features.Tickets;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace Kino.Features.Screenings
{
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
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Screening> Screenings { get; init; }
    }
}
