using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Session.Helpers;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Models;

namespace DFC.App.MatchSkills.Application.Session.Services
{
    public class SessionService : ISessionReader, ISessionWriter
    {
        private readonly CosmosService _cosmosService;
        private readonly SessionSettings _sessionSettings;

        public SessionService(ICosmosService cosmosService, IOptions<SessionSettings> sessionSettings,
            IOptions<CosmosSettings> cosmosSettings)
        {
            Throw.IfNull(cosmosService, nameof(cosmosService));
            Throw.IfNull(sessionSettings, nameof(sessionSettings));
            Throw.IfNullOrWhiteSpace(sessionSettings.Value.Salt, nameof(sessionSettings.Value.Salt));
            _sessionSettings = sessionSettings.Value;
        }

        public string CreateUserSession(string previousPage, string currentPage)
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

            var result = _cosmosService.CreateDocumentAsync(userSession);
            if (result.IsCompletedSuccessfully)
            {
                return sessionId;
            }

            return null;
        }
    }
}
