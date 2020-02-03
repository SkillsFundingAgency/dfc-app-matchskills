using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionWriter
    {
        Task<string> CreateUserSession(string previousPage, string currentPage);
        Task<HttpResponseMessage> UpdateUserSessionAsync(UserSession updatedSession);
    }
}
