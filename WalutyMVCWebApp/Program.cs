using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.DatabaseLoading.Updater;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyMVCWebApp
{
    public class Program
    {

        public  static int Main(string[] args)
        {
           var hostBuilder = CreateWebHostBuilder(args).Build();          

            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
           .Enrich.FromLogContext()
           .WriteTo.RollingFile(@"C:\Logs\log-{Date}.txt")
           .CreateLogger();

            using (var scope = hostBuilder.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<WalutyDBContext>();
                    var loader = services.GetRequiredService<ILoader>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    var currencyFileSDownloader = services.GetRequiredService<ICurrencyFilesDownloader>();

                    DBInitialisation.InitialiseDB(context, loader);
                    DefaultRolesInitialisation.Init(roleManager);
                    DefaultAdminCreator.CreateAdmin(userManager);
                    DefaultUsersCreator.CreateUsers(userManager);
                    currencyFileSDownloader.DownloadFilesAsync();

                }
                catch (Exception)
                {
                    Log.Fatal("Failed to initalise DB");
                }
            }

            try
            {
                Log.Information("Starting web host");
                hostBuilder.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
