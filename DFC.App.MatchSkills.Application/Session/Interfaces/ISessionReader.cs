using DFC.App.MatchSkills.Application.Session.Models;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Session.Interfaces
{
    public interface ISessionReader
    {
        Task<UserSession> GetUserSession(string sessionId);
    }
}
