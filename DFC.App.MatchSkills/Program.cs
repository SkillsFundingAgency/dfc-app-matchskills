using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DFC.App.MatchSkills
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                }).ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddApplicationInsights(hostingContext.Configuration.GetSection("ApplicationInsights:InstrumentationKey").Value);
                });
    }
}
