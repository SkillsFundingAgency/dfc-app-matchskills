using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosService
    {
        Task<HttpResponseMessage> CreateItemAsync(object item);
        Task<UserSession> GetUserSessionAsync(string id);
        Task<HttpResponseMessage> UpdateUserSessionAsync(UserSession updatedSession);
    }
}
