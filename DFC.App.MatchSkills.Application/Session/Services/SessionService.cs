using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Session.Helpers;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Models;

namespace DFC.App.MatchSkills.Application.Session.Services
{
    public class SessionService : ISessionReader, ISessionWriter
    {
        private readonly ICosmosService _cosmosService;
        private readonly SessionSettings _sessionSettings;

        public SessionService(ICosmosService cosmosService, IOptions<SessionSettings> sessionSettings)
        {
            Throw.IfNull(cosmosService, nameof(cosmosService));
            Throw.IfNull(sessionSettings, nameof(sessionSettings));
            Throw.IfNullOrWhiteSpace(sessionSettings.Value.Salt, nameof(sessionSettings.Value.Salt));
            _cosmosService = cosmosService;
            _sessionSettings = sessionSettings.Value;
        }

        public async Task<string> CreateUserSession(string previousPage, string currentPage)
        {
            var sessionId = SessionIdHelper.GenerateSessionId(_sessionSettings.Salt, DateTime.UtcNow);
            var partitionKey = PartitionKeyHelper.UserSession(sessionId);
            var userSession = new UserSession()
            {
                UserSessionId = sessionId,
                PartitionKey = partitionKey,
                Salt = _sessionSettings.Salt,
                CurrentPage = currentPage,
                PreviousPage = previousPage,
                LastUpdatedUtc = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            };

            var result = await _cosmosService.CreateItemAsync(userSession);
            return result.IsSuccessStatusCode ? userSession.PrimaryKey : null;
        }
    }
}
