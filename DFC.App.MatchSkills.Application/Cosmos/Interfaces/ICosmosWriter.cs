using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Services;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosWriter
    {
        Task<HttpResponseMessage> CreateItemAsync(object item, CosmosCollection collection);
        Task<HttpResponseMessage> UpsertItemAsync(object item, CosmosCollection collection);
    }
}
