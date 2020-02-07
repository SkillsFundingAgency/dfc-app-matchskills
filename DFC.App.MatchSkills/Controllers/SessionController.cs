using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{
    /// <summary>
    /// Provides common session cookie functionality for a controller
    /// </summary>
    public abstract class SessionController : Controller
    {
        private const string CookieName = ".matchSkills-session";
        private readonly IDataProtector _dataProtector;

        protected SessionController(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(nameof(SessionController));
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
                primaryKey = _dataProtector.Unprotect(cookiePrimaryKey);
            }

            var queryDictionary = System.Web.HttpUtility.ParseQueryString(request.QueryString.ToString());
            var code = queryDictionary.Get("sessionId");
            if (!string.IsNullOrEmpty(code))
            {
                primaryKey = code;
            }

            return string.IsNullOrWhiteSpace(primaryKey) ? null : primaryKey;
        }
    }
}