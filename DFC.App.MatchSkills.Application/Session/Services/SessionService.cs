using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Session.Helpers;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace DFC.App.MatchSkills.Application.Session.Services
{
    public class SessionService : ISessionService
    {
        private readonly ICosmosService _cosmosService;
        private readonly IOptions<SessionSettings> _sessionSettings;

        public SessionService(ICosmosService cosmosService, IOptions<SessionSettings> sessionSettings)
        {
            Throw.IfNull(cosmosService, nameof(cosmosService));
            Throw.IfNull(sessionSettings, nameof(sessionSettings));
            Throw.IfNullOrWhiteSpace(sessionSettings.Value.Salt, nameof(sessionSettings.Value.Salt));
            _cosmosService = cosmosService;
            _sessionSettings = sessionSettings;
        }

        public async Task<string> CreateUserSession(string previousPage, string currentPage, string sessionIdFromCookie = null)
        {
            var sessionId = string.Empty;
            var partitionKey = string.Empty;

            if (string.IsNullOrWhiteSpace(sessionIdFromCookie))
            {
                sessionId = SessionIdHelper.GenerateSessionId(_sessionSettings.Value.Salt, DateTime.UtcNow);
                partitionKey = PartitionKeyHelper.UserSession(sessionId);
            }
            else
            {
                sessionId = ExtractSessionIdFromPrimaryKey(sessionId);
                partitionKey = ExtractPartitionKeyFromPrimaryKey(sessionIdFromCookie);
            }

            var userSession = new UserSession()
            {
                UserSessionId = sessionId,
                PartitionKey = partitionKey,
                Salt = _sessionSettings.Value.Salt,
                CurrentPage = currentPage,
                PreviousPage = previousPage,
                LastUpdatedUtc = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            };

            var result = await _cosmosService.CreateItemAsync(userSession);
            return result.IsSuccessStatusCode ? userSession.PrimaryKey : null;
        }

        public Task<HttpResponseMessage> UpdateUserSessionAsync(UserSession updatedSession)
        {
            Throw.IfNull(updatedSession, nameof(updatedSession));
            return _cosmosService.UpsertItemAsync(updatedSession);
        }

        public async Task<UserSession> GetUserSession(string sessionId)
        {
            Throw.IfNullOrWhiteSpace(sessionId, nameof(sessionId));
            var result = await _cosmosService.ReadItemAsync(sessionId);
            return result.IsSuccessStatusCode ? 
                JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync()) 
                : null;
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

        public string ExtractPartitionKeyFromPrimaryKey(string sessionId)
        {
            return string.IsNullOrWhiteSpace(sessionId) ? null : sessionId.Split('-')[0];
        }
        public string ExtractSessionIdFromPrimaryKey(string sessionId)
        {
            return string.IsNullOrWhiteSpace(sessionId) ? null : sessionId.Split('-')[1];
        }

    }
}
