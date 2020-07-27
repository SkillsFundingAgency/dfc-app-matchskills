using System.Diagnostics.CodeAnalysis;
using DFC.Compui.Telemetry.HostExtensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace DFC.App.MatchSkills
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args);
            webHost.Build().AddApplicationTelemetryInitializer().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                 .ConfigureLogging((hostingContext, builder) =>
                 {
                     builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Trace);
                 })
                 .UseStartup<Startup>();
    }
}
