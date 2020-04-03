using System;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Service
{
    public static class ErrorService
    {
        public static async Task LogException(HttpContext context, ISessionService sessionService, ILogger logger)
        {
            var exception =
                context.Features.Get<IExceptionHandlerPathFeature>();

            UserSession session = null;
            try
            {
                session = await sessionService.GetUserSession();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Could not get SessionId. {ex.Message}");
            }

            logger.Log(LogLevel.Error, $"MatchSkills Error: {exception.Error.Message} \r\n" +
                                       $"Path: {exception.Path} \r\n" +
                                       $"SessionId: {(session != null ? session.UserSessionId : "Unable to get sessionId")}");
        }
    }
}
