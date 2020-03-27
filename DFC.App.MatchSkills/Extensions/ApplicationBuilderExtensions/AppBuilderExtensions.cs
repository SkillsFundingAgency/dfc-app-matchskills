using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace DFC.App.MatchSkills.Extensions.ApplicationBuilderExtensions
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder ErrorHandlingMiddleware(this IApplicationBuilder app, ILogger<Startup> logger, ISessionService sessionService)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var session = await sessionService.GetUserSession();
                    var exception =
                        context.Features.Get<IExceptionHandlerPathFeature>();
                    logger.Log(LogLevel.Error, $"MatchSkills Error: {exception.Error.Message} \r\n" +
                                               $"Path: {exception.Path} \r\n" +
                                               $"SessionId: {(session != null ? session.UserSessionId : "Unable to get sessionId")}");
                });
            });
            return app;
        }
    }
}
