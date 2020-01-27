using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace DFC.App.MatchSkills.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string CookieName = ".matchSkills-session";
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IDataProtector _dataProtector;
        public NameValueCollection QueryDictionary { get; private set; }

        protected BaseController(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(BaseController));
        }

        public abstract IActionResult Head();
        public abstract IActionResult Breadcrumb();
        public abstract IActionResult BodyTop();
        public abstract IActionResult Body();
        public abstract IActionResult SidebarRight();

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

        protected async Task<string> TryGetSessionId(HttpRequest request)
        {
            string sessionId = string.Empty;

            if (request.Cookies.TryGetValue(CookieName, out var cookieSessionId))
            {
                sessionId = _dataProtector.Unprotect(cookieSessionId);
            }

            QueryDictionary = System.Web.HttpUtility.ParseQueryString(request.QueryString.ToString());
            var code = QueryDictionary.Get("sessionId");

            if (string.IsNullOrEmpty(code) == false)
            {
                sessionId = code;
            }


            return String.IsNullOrWhiteSpace(sessionId) ? null : sessionId;
        }
        protected string ReturnPath(string segmentName, string path = "")
        {
            return $"/Views/{(string.IsNullOrWhiteSpace(path) ? "home" : path)}/{segmentName}.cshtml";
        }
    }
}
