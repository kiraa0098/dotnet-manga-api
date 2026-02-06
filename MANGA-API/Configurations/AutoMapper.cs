using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MANGA_API.Configurations
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
