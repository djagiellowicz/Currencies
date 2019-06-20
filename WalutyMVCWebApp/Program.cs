﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;


namespace WalutyMVCWebApp
{
    public class Program
    {

        public static int Main(string[] args)
        {
           var hostBuilder = CreateWebHostBuilder(args).Build();          

            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
           .Enrich.FromLogContext()
           .WriteTo.RollingFile("./logs/log-{Date}.txt")
           .CreateLogger();

            using (var scope = hostBuilder.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<WalutyDBContext>();
                    var loader = services.GetRequiredService<ILoader>();

                    DBInitialization.InitialiseDB(context, loader);
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
