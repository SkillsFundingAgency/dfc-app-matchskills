using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Cosmos.Interfaces
{
    public interface ICosmosService
    {
        Task<HttpResponseMessage> CreateItemAsync(object item);
    }
}
