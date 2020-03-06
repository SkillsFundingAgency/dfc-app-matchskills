using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    /// <summary>
    /// Provides common session cookie functionality for a controller
    /// </summary>
    public abstract class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly ICookieService _cookieService;
        

        protected SessionController(ISessionService sessionService, ICookieService cookieService)
        {
            _sessionService = sessionService;
            _cookieService = cookieService;
        }

        protected async Task CreateUserSession(CreateSessionRequest request, string sessionIdFromCookie)
        {
            await _sessionService.CreateUserSession(request, sessionIdFromCookie);
        }


        protected string TryGetPrimaryKey(HttpRequest request)
        {
            return _cookieService.TryGetPrimaryKey(request, Response);
        }

        protected async Task<HttpResponseMessage> UpdateUserSession(string sessionId, string currentPage, UserSession session = null)
        {
            if (session == null)
            {
                 session = await _sessionService.GetUserSession(sessionId);
            }
            
            session.PreviousPage = session.CurrentPage;
            session.CurrentPage = currentPage;
            session.LastUpdatedUtc = DateTime.UtcNow;

            return await _sessionService.UpdateUserSessionAsync(session);
        }

        protected async Task<UserSession> GetUserSession()
        {
            return await _sessionService.GetUserSession();
        }
    }
}