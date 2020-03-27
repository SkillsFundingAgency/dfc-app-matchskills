using System;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.AspNetCore.Diagnostics;
using NSubstitute.ExceptionExtensions;

namespace DFC.App.MatchSkills.Test.Unit.Services
{
    public class ErrorServiceTests
    {
        [Test]
        public async Task WhenNoSession_ShowNoSessionId()
        {
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Features.Get<IExceptionHandlerPathFeature>()
                .Returns(new ExceptionHandlerFeature() {Error = new Exception("Exception"), Path = "/Path"});
            var sessionService = Substitute.For<ISessionService>();
            sessionService.GetUserSession().ThrowsForAnyArgs(new Exception("message"));
            var logger = Substitute.For<ILogger<Startup>>();
            await ErrorService.LogException(httpContext, sessionService, logger);
        }
        [Test]
        public async Task WhenSession_ShowSessionId()
        {
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Features.Get<IExceptionHandlerPathFeature>()
                .Returns(new ExceptionHandlerFeature() { Error = new Exception("Exception"), Path = "/Path" });
            var sessionService = Substitute.For<ISessionService>();
            sessionService.GetUserSession().Returns(new UserSession() {UserSessionId = "SessionId"});
            var logger = Substitute.For<ILogger<Startup>>();
            await ErrorService.LogException(httpContext, sessionService, logger);
        }
    }
}
