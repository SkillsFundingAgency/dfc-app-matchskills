using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Session.Models;
using Dfc.ProviderPortal.Packages;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace DFC.App.MatchSkills.Application.Cosmos.Services
{
    public class CosmosService : ICosmosService
    {
        private readonly CosmosSettings _settings;
        private readonly CosmosClient _client;
        public CosmosService(CosmosSettings settings)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.ApiUrl, nameof(settings.ApiUrl));
            Throw.IfNullOrWhiteSpace(settings.ApiKey, nameof(settings.ApiKey));
            _settings = settings;


            _client = new CosmosClient(accountEndpoint:settings.ApiUrl, authKeyOrResourceToken:settings.ApiKey);
        }
        public async Task CreateDocumentAsync(string databaseId, string containerId, object item)
        {
            var container = _client.GetContainer(databaseId, containerId);
            await container.CreateItemAsync(item);
        }
    }
}
