using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Startup
{
    public class StartupTests
    {
        [Test]
        public void WhenConfigureApp_AddHeaders()
        {
            var config = new Mock<IConfiguration>();
            var servicerProvider = new Mock<IServiceProvider>();
            var applicationBuilder = new Mock<IApplicationBuilder>();
           
            var startup = new MatchSkills.Startup(config.Object);
            startup.ConfigureApp(applicationBuilder.Object);
        }

    }
}
