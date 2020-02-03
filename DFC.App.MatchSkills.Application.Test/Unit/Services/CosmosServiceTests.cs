using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class CosmosServiceTests
    {
        public class CreateDocumentAsyncTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            //private CosmosClient _client;
            private CosmosService _service;
            private string _currentPage, _previousPage;

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
                //_client = new CosmosClient(accountEndpoint: _cosmosSettings.Value.ApiUrl,
                //    authKeyOrResourceToken: _cosmosSettings.Value.ApiKey);
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
            //        UserSessionId = "Session14",
            //        PartitionKey = "Key",
            //        CurrentPage = "string",
            //        DysacJobCategories = new string[1],
            //        LastUpdatedUtc = DateTime.Now.ToString(),
            //        Occupation = "string",
            //        PreviousPage = "previous",
            //        Salt = "salt",
            //        RouteIncludesDysac = true,
            //        Skills = new Skill[1],
            //        UserHasWorkedBefore = true
            //    };
            //    var result = _service.CreateItemAsync(userSession).Result;
            //    result.Should().NotBeNull();
            //    result.StatusCode.Should().Be(HttpStatusCode.Created);
            //}
        }
    }
}
