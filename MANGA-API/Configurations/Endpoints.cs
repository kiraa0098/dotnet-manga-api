using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace MANGA_API.Configurations
{
    public static class Endpoints
    {
        public static void RegisterEndpoints(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
        }

        public static void ConfigureEndpoints(this WebApplication app)
        {
            app.MapControllers();
        }
    }
}
