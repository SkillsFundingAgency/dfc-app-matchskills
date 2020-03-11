using System.Configuration;
using System.IO;
using DFC.App.MatchSkills.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace DFC.App.MatchSkills.Controllers
{
    public class DynamicRouteAttribute : RouteAttribute
    {
        public DynamicRouteAttribute(string template) : base(FillTemplate(template))
        {
        }

        private static string FillTemplate(string template)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var appPath = config["CompositeSettings:Path"];

            //var appPath =  ConfigurationManager.AppSettings["CompositeSettings:Path"];
            return template.Replace("{apppath}", appPath);
        }
    }

}


