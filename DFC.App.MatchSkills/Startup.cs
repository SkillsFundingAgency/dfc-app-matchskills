using System.Diagnostics.CodeAnalysis;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DFC.App.MatchSkills
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IServiceTaxonomySearcher, ServiceTaxonomyRepository>();
            services.AddScoped<IServiceTaxonomyReader, ServiceTaxonomyRepository>();
            services.Configure<ServiceTaxonomySettings>(Configuration.GetSection(nameof(ServiceTaxonomySettings)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [ExcludeFromCodeCoverage]
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureApp(app);
        }

        public void ConfigureApp(IApplicationBuilder app)
        {
            
            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                context.Response.Headers["X-Content-Type-Options"] ="nosniff";
                context.Response.Headers["X-Xss-Protection"] = "1; mode=block";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                context.Response.Headers["Feature-Policy"] = "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'";

                //CSP
                context.Response.Headers["Content-Security-Policy"] =
                                                "default-src    'self' " +
                                                " https://www.google-analytics.com/" +
                                                    ";" +
                                                "style-src      'self' 'unsafe-inline' " +
                                                    
                                                    " https://www.googletagmanager.com/" +
                                                    " https://tagmanager.google.com/" +
                                                    " https://fonts.googleapis.com/" +
                                                ";" +
                                                "font-src       'self' data:" +
                                                   " https://fonts.googleapis.com/" +
                                                   " https://fonts.gstatic.com/" +
                                                ";" +
                                                "script-src     'self' 'unsafe-eval' 'unsafe-inline'  " +
                                                    " https://cdnjs.cloudflare.com/" +
                                                    " https://www.googletagmanager.com/" +
                                                    " https://tagmanager.google.com/" +
                                                    " https://www.google-analytics.com/" +
                                                ";";

                context.Response.GetTypedHeaders().CacheControl =
                  new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                  {
                      NoCache = true,
                      NoStore = true,
                      MustRevalidate = true,
                  };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Pragma: no-cache" };

                await next();
            });
        }
    }
}
