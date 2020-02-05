using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosWriter
    {
        Task<HttpResponseMessage> CreateItemAsync(object item);
        Task<HttpResponseMessage> UpsertItemAsync(object item);
    }
}
