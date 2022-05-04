using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kino.Data;

#pragma warning disable CS8618
public class Claim
{
    public const string Admin = nameof(Admin);
    
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

public partial class KinoDbContext
{
    public DbSet<Claim> Claims { get; init; }
}