using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Session.Helpers;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Dfc.Session;
using Dfc.Session.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DFC.App.MatchSkills.Application.Session.Services
{
    public class SessionService : ISessionService
    {
        private readonly ICosmosService _cosmosService;
        private readonly IOptions<SessionSettings> _sessionSettings;
        private readonly ISessionClient _sessionClient;

        public enum ExtractMode
        {
            PartitionKey = 0,
            SessionId = 1
        }
        public SessionService(ICosmosService cosmosService, IOptions<SessionSettings> sessionSettings,ISessionClient sessionClient )
        {
            Throw.IfNull(cosmosService, nameof(cosmosService));
            Throw.IfNull(sessionSettings, nameof(sessionSettings));
            Throw.IfNullOrWhiteSpace(sessionSettings.Value.Salt, nameof(sessionSettings.Value.Salt));
            _cosmosService = cosmosService;
            _sessionSettings = sessionSettings;
            _sessionClient = sessionClient;
        }

        public async Task<string> CreateUserSession(CreateSessionRequest request, string sessionIdFromCookie = null)
        {
            
            if (request == null)
                request = new CreateSessionRequest();

            //Create new Session here
            var dfcUserSession = _sessionClient.NewSession();
             _sessionClient.CreateCookie(dfcUserSession,true);

            var userSession = new UserSession()
            {
                UserSessionId = dfcUserSession.SessionId,
                PartitionKey = dfcUserSession.PartitionKey,
                Salt = _sessionSettings.Value.Salt,
                CurrentPage = request.CurrentPage,
                PreviousPage = request.PreviousPage,
                UserHasWorkedBefore = request.UserHasWorkedBefore,
                RouteIncludesDysac = request.RouteIncludesDysac,
                LastUpdatedUtc = DateTime.UtcNow,
            };

            var result = await _cosmosService.CreateItemAsync(userSession);
            return result.IsSuccessStatusCode ? userSession.PrimaryKey : null;
        }

        public async Task<HttpResponseMessage> UpdateUserSessionAsync(UserSession updatedSession)
        {
            Throw.IfNull(updatedSession, nameof(updatedSession));
            return await _cosmosService.UpsertItemAsync(updatedSession);
        }

        public async Task<UserSession> GetUserSession(string primaryKey)
        {
            var sesionCode = _sessionClient.TryFindSessionCode();

            Throw.IfNullOrWhiteSpace(primaryKey, nameof(primaryKey));

            var sessionId = ExtractInfoFromPrimaryKey(primaryKey, ExtractMode.SessionId);
            var partitionKey = ExtractInfoFromPrimaryKey(primaryKey, ExtractMode.PartitionKey);

            var result = await _cosmosService.ReadItemAsync(sessionId, partitionKey);
            return result.IsSuccessStatusCode ? 
                JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync()) 
                : null;
        }

        public async Task<UserSession> GetUserSession()
        { 
            var sesionCode = _sessionClient.TryFindSessionCode().Result;

           var sessionId = ExtractInfoFromPrimaryKey(sesionCode, ExtractMode.SessionId);
           var partitionKey = ExtractInfoFromPrimaryKey(sesionCode, ExtractMode.PartitionKey);
           var result = await _cosmosService.ReadItemAsync(sessionId, partitionKey);
           return result.IsSuccessStatusCode ? JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync()) : null;
        }

        public async Task<bool> CheckForExistingUserSession(string primaryKey)
        {
            if (string.IsNullOrWhiteSpace(primaryKey))
                return false;

            var result = await GetUserSession(primaryKey);

            if (result == null)
                return false;

            if (string.IsNullOrWhiteSpace(result.UserSessionId) || string.IsNullOrWhiteSpace(result.PartitionKey))
                return false;
            
            return primaryKey == result.PrimaryKey;
        }
        public string GeneratePrimaryKey()
        {
            var sessionId = SessionIdHelper.GenerateSessionId(_sessionSettings.Value.Salt, DateTime.UtcNow);
            var partitionKey = PartitionKeyHelper.UserSession(sessionId);
            var userSession = new UserSession()
            {
                UserSessionId = sessionId,
                PartitionKey =  partitionKey
            };
            return userSession.PrimaryKey;
        }

        public string ExtractInfoFromPrimaryKey(string primaryKey, ExtractMode mode)
        {
            if (string.IsNullOrWhiteSpace(primaryKey))
                return null;
            if (!primaryKey.Contains('-'))
                return null;

            return primaryKey.Split('-')[(int)mode];
        }
    }
}
