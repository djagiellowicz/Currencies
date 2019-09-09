using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;

[assembly: HostingStartup(typeof(WalutyMVCWebApp.Areas.Identity.IdentityHostingStartup))]
namespace WalutyMVCWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDefaultIdentity<User>()
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<WalutyDBContext>();
            });
        }
    }
}