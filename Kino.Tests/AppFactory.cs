using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kino.Tests;

[UsedImplicitly]
public class AppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(KinoDbContext));
            services.RemoveAll(typeof(DbContextOptions<KinoDbContext>));

            services.AddDbContext<KinoDbContext>((serviceProvider, options) =>
            {
                var dbOptions = serviceProvider
                    .GetRequiredService<IConfiguration>()
                    .GetOptions<DbOptions>();
            
                options.UseNpgsql(dbOptions.TestsConnectionString, contextOptions =>
                {
                    contextOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });
            
        });
    }
}