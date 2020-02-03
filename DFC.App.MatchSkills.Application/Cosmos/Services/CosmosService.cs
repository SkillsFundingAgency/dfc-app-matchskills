using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Cosmos.Services
{
    public class CosmosService : ICosmosService
    {
        private readonly CosmosSettings _settings;
        private readonly CosmosClient _client;
        public CosmosService(IOptions<CosmosSettings> settings, CosmosClient client)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            _settings = settings.Value;

            
            _client = client;
        }
        public async Task<HttpResponseMessage> CreateItemAsync(object item)
        {
            Throw.IfNull(item, nameof(item));

            var container = _client.GetContainer(_settings.DatabaseName, _settings.UserSessionsCollection);
            Throw.IfNull(container, nameof(container));

            var result =  container.CreateItemAsync(item).Result;

            if (result.StatusCode == HttpStatusCode.Created) 
                return new HttpResponseMessage(HttpStatusCode.Created);

            return null;
        }
    }
}
