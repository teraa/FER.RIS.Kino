using Microsoft.EntityFrameworkCore;

namespace Kino.Data;

#pragma warning disable CS8618
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

public partial class KinoDbContext
{
    public DbSet<Screening> Screenings { get; init; }
}
