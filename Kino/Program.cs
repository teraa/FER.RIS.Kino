using System.Text;
using FluentValidation.AspNetCore;
using Kino;
using Kino.Initializers;
using Kino.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtOptions = builder.Configuration.GetOptions<JwtOptions>();
        var keyBytes = Encoding.ASCII.GetBytes(jwtOptions.SigningKey);
        var key = new SymmetricSecurityKey(keyBytes);

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ClockSkew = jwtOptions.ClockSkew,
            ValidateLifetime = builder.Environment.IsProduction(),
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{nameof(Kino)}.xml"));

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        // "Bearer " is automatically prepended to the value provided in the Swagger UI for SecuritySchemeType.Http
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                },
            }
        ] = Array.Empty<string>()
    });
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

    app.UseReDoc(setup =>
    {
        setup.RoutePrefix = "redoc";
        setup.ExpandResponses("200,201");
        setup.NativeScrollbars();
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.InitAsync();
await app.RunAsync();
