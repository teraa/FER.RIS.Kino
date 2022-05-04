namespace Kino;

public static class Extensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
    {
        return configuration.GetOptionsSection<TOptions>().Get<TOptions>();
    }

    public static IServiceCollection AddOptionsWithSection<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class
    {
        return services.Configure<TOptions>(configuration.GetOptionsSection<TOptions>());
    }

    private static IConfigurationSection GetOptionsSection<TOptions>(this IConfiguration configuration)
    {
        const string suffix = "Options";

        string name = typeof(TOptions).Name;

        if (name.EndsWith(suffix))
            name = name[..^suffix.Length];

        return configuration.GetRequiredSection(name);
    }
}
