using Kino.Features.Films;
using Kino.Features.Reviews;
using Kino.Features.Screenings;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace Kino.Features.Films
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public string[] Genres { get; set; }

        public ICollection<Screening> Screenings { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Film> Films { get; init; }
    }
}
