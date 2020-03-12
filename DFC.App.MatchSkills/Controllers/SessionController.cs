using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
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
       

        protected SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        protected async Task CreateUserSession(CreateSessionRequest request)
        {
            await _sessionService.CreateUserSession(request);
        }

        protected async Task<HttpResponseMessage> UpdateUserSession(string currentPage, UserSession session = null)
        {
            if (session == null)
            {
                 session = await _sessionService.GetUserSession();
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