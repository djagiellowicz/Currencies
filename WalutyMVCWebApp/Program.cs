﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.LoadingFromFile.DatabaseLoading;

namespace WalutyMVCWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var hostBuilder = CreateWebHostBuilder(args).Build();

            using (var scope = hostBuilder.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<WalutyDBContext>();
                var loader = services.GetRequiredService<ILoader>();

                DBInitialization.InitialiseDB(context, loader);
            }

            hostBuilder.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
