using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Application.Session.Services;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionReader
    {
        Task<UserSession> GetUserSession(string primaryKey);
        Task<UserSession> GetUserSession();
        Task<bool> CheckForExistingUserSession(string primaryKey);
        string ExtractInfoFromPrimaryKey(string primaryKey, SessionService.ExtractMode mode);
    }
}
