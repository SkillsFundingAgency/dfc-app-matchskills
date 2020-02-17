using Microsoft.AspNetCore.Http;

namespace DFC.App.MatchSkills.Interfaces
{
    public interface ICookieService
    {
        string TryGetPrimaryKey(HttpRequest request, HttpResponse response);
        void AppendCookie(string sessionId, HttpResponse response);
    }
}
