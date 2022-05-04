using Microsoft.EntityFrameworkCore;

namespace Kino.Data;

#pragma warning disable CS8618
public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string[] Genres { get; set; }

    public ICollection<Screening> Screenings { get; set; }
    public ICollection<Review> Reviews { get; set; }
}

public partial class KinoDbContext
{
    public DbSet<Film> Films { get; init; }
}
