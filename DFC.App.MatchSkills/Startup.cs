using Dfc.ProviderPortal.Packages;
using Dfc.Session;
using Dfc.Session.Models;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.LMI.Interfaces;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.LMI.Services;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Services;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Service;
using DFC.App.MatchSkills.Services.Dysac;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.Configure<SessionConfig>(Configuration.GetSection(nameof(SessionConfig)));
            services.Configure<PageSettings>(Configuration.GetSection(nameof(PageSettings)));
            services.Configure<LmiSettings>(Configuration.GetSection(nameof(LmiSettings)));
            services.Configure<DysacSettings>(Configuration.GetSection(nameof(DysacSettings)));
            services.AddScoped((x) => new CosmosClient(
                accountEndpoint: Configuration.GetSection("CosmosSettings:ApiUrl").Value, 
                authKeyOrResourceToken: Configuration.GetSection("CosmosSettings:ApiKey").Value));
            services.AddScoped<ICosmosService, CosmosService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILmiService, LmiService>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<IDysacSessionReader,DysacService>();

            var sessionConfig = Configuration.GetSection(nameof(SessionConfig)).Get<SessionConfig>();
            Throw.IfNull(sessionConfig, nameof(sessionConfig));
            
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
            
            app.UseRouting();

            var appPath = Configuration.GetSection("CompositeSettings:Path").Value;
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("worked", appPath + "/worked", new {controller="worked", action="body" });
                endpoints.MapControllerRoute("selectskills",appPath + "/selectskills", new { controller = "selectskills", action = "body" });
                endpoints.MapControllerRoute("basket", appPath + "/basket", new { controller = "basket", action = "submit" });
                endpoints.MapControllerRoute("confirmremove",appPath + "/confirmremove", new { controller = "confirmremove", action = "body" });
                endpoints.MapControllerRoute("occupationsearch", appPath + "/occupationsearch/getskillsforoccupation", new { controller = "occupationsearch", action = "getskillsforoccupation" });
                endpoints.MapControllerRoute("occupationsearchauto",appPath + "/occupationsearchauto", new { controller = "occupationsearch", action = "occupationsearchauto" });
                endpoints.MapControllerRoute("removed",appPath + "/removed", new { controller = "removed", action = "body" });
                endpoints.MapControllerRoute("route",appPath + "/route", new { controller = "route", action = "body" });
                endpoints.MapControllerRoute("moreskills",appPath + "/moreskills", new { controller = "moreskills", action = "body" });
                endpoints.MapControllerRoute("relatedskills",appPath + "/relatedskills", new { controller = "relatedskills", action = "body" });
                endpoints.MapControllerRoute("enterskills",appPath + "/enterskills", new { controller = "enterskills", action = "body" });
                endpoints.MapControllerRoute("reload", appPath + "/reload", new { controller = "reload", action = "body" });
            });

        }
    }
}
