using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;

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
