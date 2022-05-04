using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kino.Data;

#pragma warning disable CS8618
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public ICollection<Claim> Claims { get; set; }
    public ICollection<Review> Reviews { get; set; }
}

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}

public partial class KinoDbContext
{
    public DbSet<User> Users { get; init; }
}