using Dfc.ProviderPortal.Packages;
using Dfc.Session;
using Dfc.Session.Models;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Session.Services
{
    public class SessionService : ISessionService
    {
        private readonly ICosmosService _cosmosService;
        private readonly IOptions<SessionConfig> _sessionConfig;
        private readonly ISessionClient _sessionClient;

        public enum ExtractMode
        {
            PartitionKey = 0,
            SessionId = 1
        }

        public SessionService(ICosmosService cosmosService, IOptions<SessionConfig> sessionConfig,
            ISessionClient sessionClient)
        {
            Throw.IfNull(cosmosService, nameof(cosmosService));
            Throw.IfNull(sessionConfig, nameof(sessionConfig));
            Throw.IfNullOrWhiteSpace(sessionConfig.Value.Salt, nameof(sessionConfig.Value.Salt));
            _cosmosService = cosmosService;
            _sessionConfig = sessionConfig;
            _sessionClient = sessionClient;
        }

        public async Task<string> CreateUserSession(CreateSessionRequest request)
        {

            if (request == null)
                request = new CreateSessionRequest();

            //Create new Session here
            var dfcUserSession = _sessionClient.NewSession();
            _sessionClient.CreateCookie(dfcUserSession, true);

            var userSession = new UserSession()
            {
                UserSessionId = dfcUserSession.SessionId,
                PartitionKey = dfcUserSession.PartitionKey,
                Salt = dfcUserSession.Salt,
                CurrentPage = request.CurrentPage,
                PreviousPage = request.PreviousPage,
                UserHasWorkedBefore = request.UserHasWorkedBefore,
                RouteIncludesDysac = request.RouteIncludesDysac,
                LastUpdatedUtc = DateTime.UtcNow,
                SessionCreatedDate = dfcUserSession.CreatedDate
            };

            var result = await _cosmosService.CreateItemAsync(userSession, CosmosCollection.Session);
            return result.IsSuccessStatusCode ? userSession.PrimaryKey : null;
        }

        public async Task<HttpResponseMessage> UpdateUserSessionAsync(UserSession updatedSession)
        {
            Throw.IfNull(updatedSession, nameof(updatedSession));
            return await _cosmosService.UpsertItemAsync(updatedSession, CosmosCollection.Session);
        }


        public async Task<UserSession> GetUserSession()
        {
            var sesionCode = _sessionClient.TryFindSessionCode().Result;
            var sessionId = ExtractInfoFromPrimaryKey(sesionCode, ExtractMode.SessionId);
            var partitionKey = ExtractInfoFromPrimaryKey(sesionCode, ExtractMode.PartitionKey);
            var result = await _cosmosService.ReadItemAsync(sessionId, partitionKey, CosmosCollection.Session);
            return result.IsSuccessStatusCode ? JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync()) : null;
        }

        public async Task<UserSession> Reload(string sessionId)
        {
            var partitionKey = _sessionClient.GeneratePartitionKey(sessionId);
                var result = await _cosmosService.ReadItemAsync(sessionId, partitionKey,CosmosCollection.Session);

                if (!result.IsSuccessStatusCode) return null;

                var userSession =
                    JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync());
                var dfcUserSession = new DfcUserSession() { Salt = userSession.Salt, 
                    PartitionKey = userSession.PartitionKey, SessionId = userSession.UserSessionId };
                _sessionClient.CreateCookie(dfcUserSession, false);
                return userSession;
        }

        public async Task<bool> CheckForExistingUserSession(string primaryKey)
        {
            if (String.IsNullOrWhiteSpace(primaryKey))
                return false;

            var result = await GetUserSession();

            if (result == null)
                return false;

            if (String.IsNullOrWhiteSpace(result.UserSessionId) || String.IsNullOrWhiteSpace(result.PartitionKey))
                return false;

            return primaryKey == result.PrimaryKey;
        }

        public string ExtractInfoFromPrimaryKey(string primaryKey, ExtractMode mode)
        {
            if (String.IsNullOrWhiteSpace(primaryKey))
                return null;
            if (!primaryKey.Contains('-'))
                return null;

            return primaryKey.Split('-')[(int) mode];
        }

    }
}
