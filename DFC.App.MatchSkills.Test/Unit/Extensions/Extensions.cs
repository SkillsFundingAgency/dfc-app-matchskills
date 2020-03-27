using System;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Extensions.ApplicationBuilderExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Extensions
{
    public class Extensions
    {
        [Test]
        public void CallExtension()
        {
            var logger = Substitute.For<ILogger<Startup>>();
            var sessionService = Substitute.For<ISessionService>();
            var app = AppBuilderExtensions.ErrorHandlingMiddleware(new ApplicationBuilder(Substitute.For<IServiceProvider>()), logger, sessionService);
        }
    }
}
