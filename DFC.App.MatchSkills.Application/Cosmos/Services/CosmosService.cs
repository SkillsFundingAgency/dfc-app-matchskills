using System.Linq;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.Cosmos.Services
{
    public class CosmosService : ICosmosService
    {
        private readonly CosmosSettings _settings;
        private readonly CosmosClient _client;
        public CosmosService(IOptions<CosmosSettings> settings, CosmosClient client)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.DatabaseName, nameof(settings.Value.DatabaseName));
            Throw.IfNullOrWhiteSpace(settings.Value.UserSessionsCollection, nameof(settings.Value.UserSessionsCollection));
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

        public async Task<UserSession> GetUserSessionAsync(string id)
        {
            Throw.IfNullOrWhiteSpace(id, nameof(id));

            var container = _client.GetContainer(_settings.DatabaseName, _settings.UserSessionsCollection);
            Throw.IfNull(container, nameof(container));

            var result = container.GetItemLinqQueryable<UserSession>(true)
                .Where(x => x.UserSessionId == id).AsEnumerable().FirstOrDefault();
    
            return result;
        }
        public async Task<HttpResponseMessage> UpdateUserSessionAsync(UserSession updatedSession)
        {
            Throw.IfNull(updatedSession, nameof(updatedSession));

            var container = _client.GetContainer(_settings.DatabaseName, _settings.UserSessionsCollection);
            Throw.IfNull(container, nameof(container));

            var result = container.UpsertItemAsync(updatedSession).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return null;
        }
    }
}
