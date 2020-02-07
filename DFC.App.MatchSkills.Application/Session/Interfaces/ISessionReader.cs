using DFC.App.MatchSkills.Application.Session.Models;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Services;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionReader
    {
        Task<UserSession> GetUserSession(string sessionId, string partitionKey);
        Task<bool> CheckForExistingUserSession(string sessionId);
        string ExtractInfoFromPrimaryKey(string sessionId, SessionService.ExtractMode mode);
    }
}
