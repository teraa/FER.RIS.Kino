namespace Kino.Controllers.Films;

public record Model(
    string Title,
    int DurationMinutes,
    string[] Genres);
