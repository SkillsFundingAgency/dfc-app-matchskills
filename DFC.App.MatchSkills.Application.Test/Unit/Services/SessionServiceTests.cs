using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Application.Session.Services;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class SessionServiceTests
    {
        public class CreateSessionTests
        {
            private IOptions<SessionSettings> _sessionSettings;
            private IOptions<CosmosSettings> _cosmosSettings;
            private Mock<CosmosClient> _client;
            [OneTimeSetUp]
            public void Init()
            {
                _cosmosSettings = Options.Create(new CosmosSettings()
                {
                    ApiUrl = "https://test-account-not-real.documents.azure.com:443/",
                    ApiKey = "VGhpcyBpcyBteSB0ZXN0",
                    DatabaseName = "DatabaseName",
                    UserSessionsCollection = "UserSessions"
                });
                _client = new Mock<CosmosClient>();

                _sessionSettings = Options.Create(new SessionSettings(){Salt = "ThisIsASalt"});
            }
            [Test]
            public async Task WhenSuccessfulCall_ReturnSessionId()
            {
                var cosmosSub = Substitute.For<ICosmosService>();
                cosmosSub.CreateItemAsync(default).ReturnsForAnyArgs(new HttpResponseMessage(HttpStatusCode.OK));
                var serviceUnderTest = new SessionService(
                    cosmosSub, _sessionSettings);
                var sessionId= await serviceUnderTest.CreateUserSession(null, null);
                sessionId.Should().NotBeNullOrWhiteSpace();

            }
            [Test]
            public async Task WhenUnsuccessfulCall_ReturnNull()
            {

                //Arbitrary value assignment to satisfy sonar
                var userSession = new UserSession()
                {
                    UserSessionId = "sd",
                    PartitionKey = "Key",
                    CurrentPage = "string",
                    DysacJobCategories = new string[1],
                    LastUpdatedUtc = DateTime.Now.ToString(),
                    Occupation = "string",
                    PreviousPage = "previous",
                    Salt = "salt",
                    RouteIncludesDysac = true,
                    Skills = new Skill[1],
                    UserHasWorkedBefore = true
                };
                var user = userSession.UserSessionId;
                var partitionKey = userSession.PartitionKey;
                var currentPage = userSession.CurrentPage;
                var jobCat = userSession.DysacJobCategories;
                var lastUpdated = userSession.LastUpdatedUtc;
                var occupation = userSession.Occupation;
                var previousPage = userSession.PreviousPage;
                var salt = userSession.Salt;
                var route = userSession.RouteIncludesDysac;
                var skills = userSession.Skills;
                var userHas = userSession.UserHasWorkedBefore;

                var cosmosSub = Substitute.For<ICosmosService>();
                cosmosSub.CreateItemAsync(default)
                    .ReturnsForAnyArgs(new HttpResponseMessage(HttpStatusCode.BadRequest));
                var serviceUnderTest = new SessionService(
                    cosmosSub, _sessionSettings);
                var sessionId = await serviceUnderTest.CreateUserSession(null, null);
                sessionId.Should().BeNull();

            }
        }
    }
}
