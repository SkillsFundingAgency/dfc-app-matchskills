using DFC.App.MatchSkills.Application.Session.Models;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Services;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionReader
    {
        Task<UserSession> GetUserSession(string primaryKey);
        Task<bool> CheckForExistingUserSession(string primaryKey);
        string ExtractInfoFromPrimaryKey(string primaryKey, SessionService.ExtractMode mode);
    }
}
