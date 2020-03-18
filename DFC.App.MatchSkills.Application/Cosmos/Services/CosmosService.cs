using System;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace DFC.App.MatchSkills.Application.Cosmos.Services
{
    public enum CosmosCollection
    {
        Session = 1,
        LmiData = 2
    }

    public class CosmosService : ICosmosService
    {
        private readonly CosmosSettings _settings;
        private readonly CosmosClient _client;
        public CosmosService(IOptions<CosmosSettings> settings, CosmosClient client)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.DatabaseName, nameof(settings.Value.DatabaseName));
            Throw.IfNullOrWhiteSpace(settings.Value.UserSessionsCollection, nameof(settings.Value.UserSessionsCollection));
            Throw.IfNullOrWhiteSpace(settings.Value.LmiDataCollection, nameof(settings.Value.LmiDataCollection));
            _settings = settings.Value;
            _client = client;
        }
        public async Task<HttpResponseMessage> CreateItemAsync(object item, CosmosCollection collection)
        {
            Throw.IfNull(item, nameof(item));

            var container = _client.GetContainer(_settings.DatabaseName, GetContainerName(collection));
            Throw.IfNull(container, nameof(container));

            var result =  await container.CreateItemAsync(item);

            if (result.StatusCode == HttpStatusCode.Created) 
                return new HttpResponseMessage(HttpStatusCode.Created);

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        public async Task<HttpResponseMessage> ReadItemAsync(string id, string partitionKey, CosmosCollection collection)
        {
            Throw.IfNullOrWhiteSpace(id, nameof(id));

            var container = _client.GetContainer(_settings.DatabaseName, GetContainerName(collection));
            Throw.IfNull(container, nameof(container));
            try
            {
                var result = await container.ReadItemAsync<object>(id, partitionKey.IsNullOrEmpty() ? PartitionKey.None : new PartitionKey(partitionKey));

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    if(result.Resource == null)
                        return new HttpResponseMessage(HttpStatusCode.NotFound);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(result.Resource.ToString()) 
                    };
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        public async Task<HttpResponseMessage> UpsertItemAsync(object item, CosmosCollection collection)
        {
            Throw.IfNull(item, nameof(item));

            var container = _client.GetContainer(_settings.DatabaseName, GetContainerName(collection));
            Throw.IfNull(container, nameof(container));

            var result = await container.UpsertItemAsync(item);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            if (result.StatusCode == HttpStatusCode.Created)
            {
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        internal string GetContainerName(CosmosCollection collection)
        {
            switch (collection)
            {
                case CosmosCollection.Session:
                default:
                    return _settings.UserSessionsCollection;
                
                case CosmosCollection.LmiData:
                    return _settings.LmiDataCollection;
            }
        }
    }
}
