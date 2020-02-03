using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionReader
    {
        Task<UserSession> GetUserSession(string sessionId);
    }
}
