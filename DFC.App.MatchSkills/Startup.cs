using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.Dysac;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        }
    }
}
