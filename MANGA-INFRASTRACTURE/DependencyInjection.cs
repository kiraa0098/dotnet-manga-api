using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MANGA_APPLICATION.Abstractions.External;
using MANGA_INFRASTRUCTURE.External.MangaDex;

namespace MANGA_INFRASTRUCTURE;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MangaDexOptions>(config.GetSection("MangaDex"));

        services.AddHttpClient<IMangaSource, MangaDexClient>();

        return services;
    }
}
