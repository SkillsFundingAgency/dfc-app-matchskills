using AutoMapper;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.LMI.Interfaces;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.LMI.Services;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Application.Session.Services;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Service;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Dfc.ProviderPortal.Packages;
using Dfc.Session;
using Dfc.Session.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace DFC.App.MatchSkills
{
    public class Startup
    {
        private readonly string _corsPolicy = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationInsightsTelemetry();

            services.AddControllersWithViews();
            services.AddScoped<IServiceTaxonomySearcher, ServiceTaxonomyRepository>();
            services.AddScoped<IServiceTaxonomyReader, ServiceTaxonomyRepository>();
            services.Configure<ServiceTaxonomySettings>(Configuration.GetSection(nameof(ServiceTaxonomySettings)));
            services.Configure<CompositeSettings>(Configuration.GetSection(nameof(CompositeSettings)));
            services.Configure<CosmosSettings>(Configuration.GetSection(nameof(CosmosSettings)));
            services.Configure<SessionSettings>(Configuration.GetSection(nameof(SessionSettings)));
            services.Configure<PageSettings>(Configuration.GetSection(nameof(PageSettings)));
            services.Configure<LmiSettings>(Configuration.GetSection(nameof(LmiSettings)));
            services.AddScoped((x) => new CosmosClient(
                accountEndpoint: Configuration.GetSection("CosmosSettings:ApiUrl").Value, 
                authKeyOrResourceToken: Configuration.GetSection("CosmosSettings:ApiKey").Value));
            services.AddScoped<ICosmosService, CosmosService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILmiService, LmiService>();
            var sessionSettings = Configuration.GetSection(nameof(SessionSettings)).Get<SessionSettings>();
            Throw.IfNull(sessionSettings, nameof(sessionSettings));
            var sessionConfig = new SessionConfig
            {
                ApplicationName = sessionSettings.ApplicationName,
                Salt = sessionSettings.Salt,
            };
            services.AddSessionServices(sessionConfig);

            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicy,
                    builder => builder
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            
            


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

            app.UseRouting();
            app.UseCors(_corsPolicy);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
