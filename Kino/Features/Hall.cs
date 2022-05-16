using Kino.Features;
using Kino.Features.Screenings;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace Kino.Features
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }

        public ICollection<Seat> Seats { get; set; }
        public ICollection<Screening> Screenings { get; set; }
    }
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Hall> Halls { get; init; }
    }
}
