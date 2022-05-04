using Kino;
using Kino.Data;
using Kino.Initializers;
using Kino.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDbContext<KinoDbContext>((services, options) =>
    {
        var dbOptions = services
            .GetRequiredService<IConfiguration>()
            .GetOptions<DbOptions>();

        options.UseNpgsql(dbOptions.ConnectionString, contextOptions =>
        {
            contextOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
    })
    .AddAsyncInitializer<MigrationInitializer>()
    .AddOptionsWithSection<JwtOptions>(builder.Configuration)
    .AddSingleton<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.InitAsync();
await app.RunAsync();
