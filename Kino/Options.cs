// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618
namespace Kino;

public class DbOptions
{
    public string ConnectionString { get; set; }
}

public class JwtOptions
{
    public string SigningKey { get; set; }
    public TimeSpan TokenLifetime { get; set; }
    public TimeSpan ClockSkew { get; set; }
}
