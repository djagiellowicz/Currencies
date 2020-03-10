using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
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
                    
                    var currencyFilesDownloader = services.GetRequiredService<ICurrencyFilesDownloader>();
                    var currencyFilesUnzipper = services.GetRequiredService<ICurrencyFilesUnzipper>();
                    var currencyFilesUpdater = services.GetRequiredService<ICurrencyFilesUpdater>();


                    DBInitialisation.InitialiseDB(context, loader);
                    DefaultRolesInitialisation.Init(roleManager);
                    DefaultAdminCreator.CreateAdmin(userManager);
                    DefaultUsersCreator.CreateUsers(userManager);
                    currencyFilesUpdater.Process(context);

                }
                catch (Exception e)
                {
                    Log.Fatal("Failed to initalise DB");
                    Log.Fatal(e.Message);
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
