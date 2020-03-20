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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Dysac.Models;
using Dfc.Session;
using Dfc.Session.Models;
using Newtonsoft.Json;
using NSubstitute.ReturnsExtensions;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class SessionServiceTests
    {
        public class CreateSessionTests
        {
            private IOptions<SessionConfig> _sessionConfig;
            private IOptions<CosmosSettings> _cosmosSettings;
            private Mock<CosmosClient> _client;
            private ISessionClient _sessionClient=Substitute.For<ISessionClient>();
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
                var dummySession = new DfcUserSession()
                {
                    SessionId = "partitionkey-sessionid"
                };
                
                _sessionClient.NewSession().Returns(dummySession);
                _sessionClient.CreateCookie(Arg.Any<DfcUserSession>(),true);
                _client = new Mock<CosmosClient>();
                _sessionConfig = Options.Create(new SessionConfig(){Salt = "ThisIsASalt",ApplicationName = "matchskills"});
            }
            [Test]
            public async Task WhenSuccessfulCall_ReturnSessionId()
            {
                var cosmosSub = Substitute.For<ICosmosService>();
                cosmosSub.CreateItemAsync(default, CosmosCollection.Session).ReturnsForAnyArgs(new HttpResponseMessage(HttpStatusCode.OK));
                cosmosSub.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));
                
                var serviceUnderTest = new SessionService(
                    cosmosSub, _sessionConfig,_sessionClient);
                
                var sessionId= await serviceUnderTest.CreateUserSession(null);
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
                    DysacJobCategories = new DysacJobCategory[1],
                    LastUpdatedUtc = DateTime.UtcNow,
                    Occupations = new HashSet<UsOccupation>(){ new UsOccupation("1","Occupation 1"), new UsOccupation("2","Occupation 1") },
                    PreviousPage = "previous",
                    Salt = "salt",
                    RouteIncludesDysac = true,
                    Skills = new HashSet<UsSkill>(){ new UsSkill("1","skill1"), new UsSkill("2","skill2") },
                    UserHasWorkedBefore = true
                };
                var user = userSession.UserSessionId;
                var partitionKey = userSession.PartitionKey;
                var currentPage = userSession.CurrentPage;
                var jobCat = userSession.DysacJobCategories;
                var lastUpdated = userSession.LastUpdatedUtc;
                var occupations = userSession.Occupations;
                var previousPage = userSession.PreviousPage;
                var salt = userSession.Salt;
                var route = userSession.RouteIncludesDysac;
                var skills = userSession.Skills;
                var userHas = userSession.UserHasWorkedBefore;
                
                
                var cosmosSub = Substitute.For<ICosmosService>();
                cosmosSub.CreateItemAsync(default, CosmosCollection.Session)
                    .ReturnsForAnyArgs(new HttpResponseMessage(HttpStatusCode.BadRequest));
                cosmosSub.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));

                
                var serviceUnderTest = new SessionService(
                    cosmosSub, _sessionConfig,_sessionClient);
                var sessionId = await serviceUnderTest.CreateUserSession(new CreateSessionRequest());
                sessionId.Should().BeNull();

            }
        }

        public class GetUserSessionTests
        {
            private IOptions<SessionConfig> _sessionConfig;
            private IOptions<CosmosSettings> _cosmosSettings;
            private CosmosClient _client;
            private ICosmosService _cosmosService;
            private ISessionClient _sessionClient;

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
                var dummySession = new DfcUserSession()
                {
                    SessionId = "partitionkey-sessionid"
                };
                _client = Substitute.For<CosmosClient>();
                _cosmosService = Substitute.For<ICosmosService>();
                _sessionClient = Substitute.For<ISessionClient>();
                _sessionConfig = Options.Create(new SessionConfig()
                {
                    Salt = "ThisIsASalt",
                    ApplicationName = "matchskills"
                });
                
                _sessionClient.NewSession().Returns(dummySession);
            }

            
            [Test]
            public async Task IfResultIsNotSuccess_ReturnNull()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
                var result = await serviceUnderTest.GetUserSession();

                result.Should().BeNull();
            }
            [Test]
            public async Task IfResultIsSuccess_ReturnSuccess()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent
                        (JsonConvert.SerializeObject(new UserSession()))
                    }));
                var result = await serviceUnderTest.GetUserSession();

                result.Should().NotBeNull();
            }

        }

        public class UpdateUserSessionTests
        {
            private IOptions<SessionConfig> _sessionConfig;
            private IOptions<CosmosSettings> _cosmosSettings;
            private CosmosClient _client;
            private ICosmosService _cosmosService;
            private ISessionClient _sessionClient;

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
                _client = Substitute.For<CosmosClient>();
                _cosmosService = Substitute.For<ICosmosService>();
                _sessionClient = Substitute.For<ISessionClient>();
                _sessionConfig = Options.Create(new SessionConfig(){Salt = "ThisIsASalt"});
            }

            [Test]
            public void IfUserSessionIsNull_ThrowArgumentException()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                serviceUnderTest.Invoking(x => x.UpdateUserSessionAsync(null)).Should().Throw<ArgumentException>();
            }
            [Test]
            public async Task IfResultIsNotSuccess_ReturnNull()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                _cosmosService.UpsertItemAsync(Arg.Any<UserSession>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
                var result = await serviceUnderTest.UpdateUserSessionAsync(new UserSession());

                result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }
            [Test]
            public async Task IfResultIsSuccess_ReturnSuccess()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                _cosmosService.UpsertItemAsync(Arg.Any<UserSession>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent
                        (JsonConvert.SerializeObject(new UserSession()))
                    }));
                var result = await serviceUnderTest.UpdateUserSessionAsync(new UserSession());

                result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

      
        public class ExtractInfoFromPrimaryKeyTests
        {
            private IOptions<SessionConfig> _sessionConfig;
            private ICosmosService _cosmosService;
            private ISessionClient _sessionClient;

            [OneTimeSetUp]
            public void Init()
            {
                _cosmosService = Substitute.For<ICosmosService>();
                _sessionConfig = Options.Create(new SessionConfig(){Salt = "ThisIsASalt"});
                _sessionClient = Substitute.For<ISessionClient>();
            }

            [Test]
            public void WhenSessionIdIsNullOrEmpty_ReturnNull()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                var sessionId = serviceUnderTest.ExtractInfoFromPrimaryKey(null, SessionService.ExtractMode.SessionId);
                sessionId.Should().BeNullOrWhiteSpace();

            }
            [Test]
            public void WhenSessionIdFormatIncorrect_ReturnNull()
            {
                
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                var sessionId = serviceUnderTest.ExtractInfoFromPrimaryKey("incorrectFormat", SessionService.ExtractMode.PartitionKey);
                sessionId.Should().BeNullOrWhiteSpace();

            }
            [Test]
            public void WhenPartitionKeyRequested_ReturnPartitionKey()
            {
                string primaryKey = "session5-gn84ygzmm4893m";
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                var partitionKey = serviceUnderTest.ExtractInfoFromPrimaryKey(primaryKey, SessionService.ExtractMode.PartitionKey);
                partitionKey.Should().NotBeNullOrWhiteSpace();
                partitionKey.Should().Be("session5");

            }
            [Test]
            public void WhenSessionIdRequested_ReturnSessionId()
            {
                string primaryKey = "session5-gn84ygzmm4893m";
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                var sessionId = serviceUnderTest.ExtractInfoFromPrimaryKey(primaryKey, SessionService.ExtractMode.SessionId);
                sessionId.Should().NotBeNullOrWhiteSpace();
                sessionId.Should().Be("gn84ygzmm4893m");

            }
        }

        public class CheckForExistingUserSessionTests
        {
            private IOptions<SessionConfig> _sessionConfig;
            private ICosmosService _cosmosService;
            private ISessionClient _sessionClient;

            [OneTimeSetUp]
            public void Init()
            {
                _cosmosService = Substitute.For<ICosmosService>();
                _sessionConfig = Options.Create(new SessionConfig(){Salt = "ThisIsASalt"});
                _sessionClient = Substitute.For<ISessionClient>();
            }

            [Test]
            public async Task WhenSessionIdIsNullOrEmpty_ReturnNull()
            {
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                var sessionId = await serviceUnderTest.CheckForExistingUserSession(null);
                sessionId.Should().BeFalse();

            }
            [Test]
            public async Task WhenNoUserSessionExists_ReturnFalse()
            {
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                
                var sessionId = await serviceUnderTest.CheckForExistingUserSession("session-g2454t4f");
                sessionId.Should().BeFalse();

            }
            [Test]
            public async Task WhenSuccessfulCallButEmptySessionId_ReturnFalse()
            {
                var primaryKey = "session2-g2454t4f";
                var userSession = new UserSession
                {
                    PartitionKey = "session2"
                };
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(userSession))
                }));
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                
                var sessionId = await serviceUnderTest.CheckForExistingUserSession(primaryKey);
                sessionId.Should().BeFalse();

            }
            [Test]
            public async Task WhenSuccessfulCallButEmptyPartitionKey_ReturnFalse()
            {
                var primaryKey = "session2-g2454t4f";
                var userSession = new UserSession
                {
                    UserSessionId = "g2454t4f"
                };
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(userSession))
                }));
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                
                var sessionId = await serviceUnderTest.CheckForExistingUserSession(primaryKey);
                sessionId.Should().BeFalse();

            }
            [Test]
            public async Task WhenSuccessfulCallAndPrimaryKeyMatch_ReturnTrue()
            {
                var primaryKey = "session2-g2454t4f";
                var userSession = new UserSession
                {
                    UserSessionId = "g2454t4f",
                    PartitionKey = "session2"
                };
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(userSession))
                }));
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig,_sessionClient);
                
                var sessionId = await serviceUnderTest.CheckForExistingUserSession(primaryKey);
                sessionId.Should().BeTrue();

            }

        }

        public class ReloadTests
        {
            private IOptions<SessionConfig> _sessionConfig;
            private ICosmosService _cosmosService;
            private ISessionClient _sessionClient;

            [OneTimeSetUp]
            public void Init()
            {
                _cosmosService = Substitute.For<ICosmosService>();
                _sessionConfig = Options.Create(new SessionConfig() { Salt = "ThisIsASalt" });
                _sessionClient = Substitute.For<ISessionClient>();
            }
            [Test]
            public async Task WhenUserSessionNotFound_ThenReturnNull()
            {
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .ReturnsForAnyArgs(Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig, _sessionClient);
                var result = await serviceUnderTest.Reload("123");

                result.Should().BeNull();

            }

            [Test]
            public async Task WhenUserSessionFound_ThenCreateCookieAndReturnSession()
            {
                var userSession = new UserSession
                {
                    UserSessionId = "123",
                    PartitionKey = "123",
                    Salt = "123"
                };
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), CosmosCollection.Session)
                    .ReturnsForAnyArgs(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(userSession))
                    }));
                var serviceUnderTest = new SessionService(_cosmosService, _sessionConfig, _sessionClient);
                var result = await serviceUnderTest.Reload("123");

                _sessionClient.Received(1).CreateCookie(Arg.Any<DfcUserSession>(), false);

                result.Should().NotBeNull();

            }
        }
    }
}
