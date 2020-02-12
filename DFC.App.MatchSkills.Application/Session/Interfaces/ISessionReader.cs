using DFC.App.MatchSkills.Application.Session.Models;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Services;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionReader
    {
        Task<UserSession> GetUserSession(string primaryKey);
        Task<bool> CheckForExistingUserSession(string primaryKey);
        string ExtractInfoFromPrimaryKey(string primaryKey, SessionService.ExtractMode mode);
        Task<Occupation[]> GetRecentlyAddedOccupations(string primaryKey);
    }
}
