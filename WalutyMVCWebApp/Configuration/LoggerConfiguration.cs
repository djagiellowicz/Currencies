using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace WalutyMVCWebApp.Configuration
{
    public static class LoggerConfig
    {
        public static void ConfigureLogger(this IServiceCollection services, IConfiguration configuration)
        {
            string logsFilePath = configuration.GetSection("Logger")["logsFilePath"];

            if (Path.IsPathFullyQualified(logsFilePath))
            { }
            else
            {
                logsFilePath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "log-{Date}.txt"); 
            }

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
               .Enrich.FromLogContext()
               .WriteTo.RollingFile(logsFilePath)
               .CreateLogger();
        }

    }
}
