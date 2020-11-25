using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalutyBusinessLogic.DatabaseLoading;

namespace WalutyMVCWebApp.Configuration
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        { 
            if (configuration.GetFlag("IsDevelopment"))
            {
                services.AddDbContextPool<WalutyDBContext>(opt =>
                    opt.UseInMemoryDatabase("Development"));
            }
            else
            {
                services.AddDbContext<WalutyDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("Connection")));
            }
        }
    }
}
