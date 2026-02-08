using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MANGA_API.Configurations
{
    public static class MediatorConfig
    {
        public static void AddMediator(this WebApplicationBuilder builder)
        {
            var appAssembly = typeof(MANGA_APPLICATION.Manga.Queries.SearchMangaQuery).Assembly;
            builder.Services.AddMediatR(cfg => { }, appAssembly);
        }
    }
}
