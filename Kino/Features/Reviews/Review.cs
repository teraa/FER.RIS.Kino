using Kino.Features.Films;
using Kino.Features.Reviews;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace Kino.Features.Reviews
{
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
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Review> Reviews { get; init; }
    }
}
