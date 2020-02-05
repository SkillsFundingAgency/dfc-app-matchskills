using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosReader
    {
        Task<HttpResponseMessage> ReadItemAsync(string id);
    }
}
