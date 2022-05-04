using Microsoft.AspNetCore.Mvc;

namespace Kino.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] s_summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5)
            .Select(index => new WeatherForecast(
                Date: DateTime.Now.AddDays(index),
                TemperatureC: Random.Shared.Next(-20, 55),
                Summary: s_summaries[Random.Shared.Next(s_summaries.Length)]
            ))
            .ToArray();
    }
}

public record WeatherForecast(
    DateTime Date,
    int TemperatureC,
    string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}
