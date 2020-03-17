using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Services;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosReader
    {
        Task<HttpResponseMessage> ReadItemAsync(string id, string partitionKey, CosmosCollection collection);
    }
}
