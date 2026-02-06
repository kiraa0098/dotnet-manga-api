using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MANGA_APPLICATION.Manga.Queries;
using System.Reflection;

namespace MANGA_API.Configurations
{
    public static class MediatorConfig
    {
        public static void AddMediator(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly(),
                    typeof(SearchMangaQueryHandler).Assembly
                );
            });
        }
    }
}
