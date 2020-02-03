using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Session.Models;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class CosmosServiceTests
    {
        public class CreateItemAsyncTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            private CosmosService _service;

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
            }

            [Test]
            public void WhenItemIsNull_ThrowException()
            {
                var client = Substitute.For<CosmosClient>();
                _service = new CosmosService(_cosmosSettings, client);
                _service.Invoking(x => x.CreateItemAsync(null)).Should().Throw<ArgumentException>();

            }

            [Test]
            public void WhenContainerIsNull_ThrowError()
            {
                var client = Substitute.For<CosmosClient>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection)
                    .ReturnsNullForAnyArgs();
                _service = new CosmosService(_cosmosSettings, client);
                _service.Invoking(x => x.CreateItemAsync(new UserSession())).Should().Throw<ArgumentException>();

            }
            [Test]
            public void WhenBadRequestIsMade_ReturnNull()
            {
                var client = Substitute.For<CosmosClient>();
                var container = Substitute.For<Container>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = _service.CreateItemAsync(new UserSession()).Result;
                result.Should().BeNull();

            }
            //[Test]
            //public void WhenSuccessfulRequestIsMade_ReturnHTTPOk()
            //{
            //    var client = new CosmosClient(accountEndpoint: "", authKeyOrResourceToken: "");
            //    _cosmosSettings.Value.UserSessionsCollection = "";
            //    _cosmosSettings.Value.DatabaseName = "";
            //    _service = new CosmosService(_cosmosSettings, client);
            //    var userSession = new UserSession()
            //    {
            //        UserSessionId = "gn84e8nzzxzk8z",
            //        PartitionKey = "session10",
            //        CurrentPage = "thisHasBeenupdated",
            //        DysacJobCategories = new string[1],
            //        LastUpdatedUtc = DateTime.Now.ToString(),
            //        Occupation = "string",
            //        PreviousPage = "previous",
            //        Salt = "BatteryHorseStapleCorrect",
            //        RouteIncludesDysac = true,
            //        Skills = new Skill[1],
            //        UserHasWorkedBefore = true
            //    };
            //    var result = _service.CreateItemAsync(userSession).Result;
            //    result.Should().NotBeNull();
            //    result.StatusCode.Should().Be(HttpStatusCode.Created);
            //}
        }

        public class GetUserSessionTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            private CosmosService _service;

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
            }

            [Test]
            public void WhenItemIsNull_ThrowException()
            {
                var client = Substitute.For<CosmosClient>();
                _service = new CosmosService(_cosmosSettings, client);
                _service.Invoking(x => x.GetUserSessionAsync(null)).Should().Throw<ArgumentException>();

            }

            [Test]
            public void WhenContainerIsNull_ThrowError()
            {
                var client = Substitute.For<CosmosClient>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection)
                    .ReturnsNullForAnyArgs();
                _service = new CosmosService(_cosmosSettings, client);
                _service.Invoking(x => x.GetUserSessionAsync("Id")).Should().Throw<ArgumentException>();

            }
        }

        public class UpdateUserSessionTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            private CosmosService _service;

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
            }

        }
    }
}
