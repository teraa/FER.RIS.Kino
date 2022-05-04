using Microsoft.EntityFrameworkCore;

namespace Kino.Data;

#pragma warning disable CS8618
public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }

    public ICollection<Seat> Seats { get; set; }
    public ICollection<Screening> Screenings { get; set; }
}

public partial class KinoDbContext
{
    public DbSet<Hall> Halls { get; init; }
}
