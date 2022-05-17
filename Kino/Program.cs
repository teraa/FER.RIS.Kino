using FluentValidation.AspNetCore;
using Kino;
using Kino.Initializers;
using Kino.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using KinoDbContext = Kino.KinoDbContext;

Serilog.Debugging.SelfLog.Enable(x
    => Console.WriteLine($"SERILOG: {x}"));

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseDefaultServiceProvider(options =>
    {
        options.ValidateOnBuild = true;
        options.ValidateScopes = true;
    })
    .UseSerilog((hostContext, options) =>
    {
        options.ReadFrom.Configuration(hostContext.Configuration);
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

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
    .AddSingleton<TokenService>()
    .AddSingleton<PasswordService>()
    .AddMediatR(typeof(Program))
    .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining(typeof(Program)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });

    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        setup.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseAuthorization();

app.MapControllers();

await app.InitAsync();
await app.RunAsync();
