using DFC.App.MatchSkills.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;

namespace DFC.App.MatchSkills.Service
{
    public class CookieService : ICookieService
    {
        public const string CookieName = ".dfc-session";
        private readonly IDataProtector _dataProtector;

        public CookieService(IDataProtectionProvider dataProtector)
        {
            _dataProtector = dataProtector.CreateProtector(nameof(CookieService));
        }

        private void RemoveInvalidSession(HttpResponse response)
        {
            response.Cookies.Delete(CookieName);
        }

        public string TryGetPrimaryKey(HttpRequest request, HttpResponse response)
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
                    RemoveInvalidSession(response);
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

        public void AppendCookie(string sessionId, HttpResponse response)
        {
            var value = _dataProtector.Protect(sessionId);
            response.Cookies.Append(CookieName, value, new CookieOptions
            {
                Secure = true,
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
        }
    }
}
