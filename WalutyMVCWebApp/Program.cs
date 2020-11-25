using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.DatabaseLoading.Updater;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyMVCWebApp.Configuration;

namespace WalutyMVCWebApp
{
    public class Program
    {

        public static int Main(string[] args)
        {
            var hostBuilder = CreateWebHostBuilder(args).Build();


            using (var scope = hostBuilder.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<WalutyDBContext>();
                    var loader = services.GetRequiredService<ILoader>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var configuration = services.GetRequiredService<IConfiguration>();

                    DBInitialisation.InitialiseDB(context, loader);
                    DefaultRolesInitialisation.Init(roleManager);
                    DefaultAdminCreator.CreateAdmin(userManager);
                    DefaultUsersCreator.CreateUsers(userManager);
                }
                catch (Exception e)
                {
                    Log.Fatal("Failed to initalise DB");
                    Log.Fatal(e.Message);
                }
            }

            using (var scope = hostBuilder.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;
                    var configuration = services.GetRequiredService<IConfiguration>();
                    var context = services.GetRequiredService<WalutyDBContext>();

                    // Part responsible for auto database update at start of the software. By default turned off
                    var currencyFilesDownloader = services.GetRequiredService<ICurrencyFilesDownloader>();
                    var currencyFilesUnzipper = services.GetRequiredService<ICurrencyFilesUnzipper>();
                    var currencyFilesUpdater = services.GetRequiredService<ICurrencyFilesUpdater>();

                    // Part responsible for auto database update at start of the software. By default turned off
                    if (configuration.GetFlag("IsAutomaticUpdateOn"))
                    {
                        currencyFilesUpdater.Process(context);
                    }
                }
                catch (Exception e)
                {
                    Log.Fatal("Failed to automaticaly update database");
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
