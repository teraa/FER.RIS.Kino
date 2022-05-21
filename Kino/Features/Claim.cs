using Kino.Features;
using Kino.Features.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#pragma warning disable CS8618

namespace Kino.Features
{
    public class Claim
    {
        public int UserId { get; set; }
        public string Type { get; set; }

        public User User { get; set; }
    }

public class ClaimConfig : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.HasKey(x => new {x.UserId, x.Type});
    }
}
}

namespace Kino
{
    public partial class KinoDbContext
    {
        public DbSet<Claim> Claims { get; init; }
    }
}
