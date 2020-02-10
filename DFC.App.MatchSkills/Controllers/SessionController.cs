using System;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Controllers
{
    /// <summary>
    /// Provides common session cookie functionality for a controller
    /// </summary>
    public abstract class SessionController : Controller
    {
        private const string CookieName = ".matchSkills-session";
        private readonly IDataProtector _dataProtector;
        private readonly ISessionService _sessionService;

        protected SessionController(IDataProtectionProvider dataProtectionProvider, ISessionService sessionService)
        {
            _sessionService = sessionService;
            _dataProtector = dataProtectionProvider.CreateProtector(nameof(SessionController));
        }

        protected async Task CreateUserSession(CreateSessionRequest request, string sessionIdFromCookie)
        {
            var primaryKey = await _sessionService.CreateUserSession(request, sessionIdFromCookie);

            AppendCookie(primaryKey);
        }

        protected void AppendCookie(string sessionId)
        {
            var value = _dataProtector.Protect(sessionId);
            Response.Cookies.Append(CookieName, value, new CookieOptions
            {
                Secure = true,
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
        }

        protected string TryGetPrimaryKey(HttpRequest request)
        {
            var primaryKey = string.Empty;
            if (request.Cookies.TryGetValue(CookieName, out var cookiePrimaryKey))
            {
                try
                {
                    primaryKey = _dataProtector.Unprotect(cookiePrimaryKey);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Key outdated. Removing Now. {e}");
                    RemoveInvalidSession();
                }
                
            }

            var queryDictionary = System.Web.HttpUtility.ParseQueryString(request.QueryString.ToString());
            var code = queryDictionary.Get("sessionId");
            if (!string.IsNullOrEmpty(code))
            {
                primaryKey = code;
            }

            return string.IsNullOrWhiteSpace(primaryKey) ? null : primaryKey;
        }

        protected async Task<HttpResponseMessage> UpdateUserSession(string sessionId, string currentPage )
        {
            var session = await _sessionService.GetUserSession(sessionId);

            session.PreviousPage = session.CurrentPage;
            session.CurrentPage = currentPage;
            

            return await _sessionService.UpdateUserSessionAsync(session);

        }

        protected void RemoveInvalidSession()
        {
            Response.Cookies.Delete(CookieName);
        }
    }
}