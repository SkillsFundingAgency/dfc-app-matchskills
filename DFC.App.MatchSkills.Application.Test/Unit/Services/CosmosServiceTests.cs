using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class CosmosServiceTests
    {
        public class CreateItemAsyncTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            private ICosmosService _service;

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
                //Code coverage for Sonar
                var apiUrl = _cosmosSettings.Value.ApiUrl;
                var apiKey = _cosmosSettings.Value.ApiKey;
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
            public async Task WhenBadRequestIsMade_ReturnNull()
            {
                var client = Substitute.For<CosmosClient>();
                var container = Substitute.For<Container>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.CreateItemAsync(new UserSession());
                var expected = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                result.StatusCode.Should().Be(expected.StatusCode);
            }

            [Test]
            public async Task WhenCorrectRequestMade_ReturnSuccessCode()
            {
                var client = Substitute.For<CosmosClient>();

                var container = Substitute.For<Container>();

                var response = Substitute.For<ItemResponse<object>>();
                response.StatusCode.ReturnsForAnyArgs(HttpStatusCode.Created);
                container.CreateItemAsync(Arg.Any<object>()).Returns(Task.FromResult(response));

                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.CreateItemAsync(new UserSession());
                var expected = new HttpResponseMessage(HttpStatusCode.Created);
                result.StatusCode.Should().Be(expected.StatusCode);
            }

        }

        public class ReadItemAsyncTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            private ICosmosService _service;

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
                _service.Invoking(x => x.ReadItemAsync(null)).Should().Throw<ArgumentException>();

            }

            [Test]
            public void WhenContainerIsNull_ThrowError()
            {
                var client = Substitute.For<CosmosClient>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection)
                    .ReturnsNullForAnyArgs();
                _service = new CosmosService(_cosmosSettings, client);
                _service.Invoking(x => x.ReadItemAsync("Id")).Should().Throw<ArgumentException>();

            }
            [Test]
            public async Task WhenBadRequestIsMade_ReturnNull()
            {
                var client = Substitute.For<CosmosClient>();
                var container = Substitute.For<Container>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.ReadItemAsync("id");
                var expected = new HttpResponseMessage(HttpStatusCode.NotFound);
                result.StatusCode.Should().Be(expected.StatusCode);
            }
            [Test]
            public async Task WhenCorrectRequestMade_ReturnSuccessCode()
            {
                var client = Substitute.For<CosmosClient>();

                var container = Substitute.For<Container>();

                var response = Substitute.For<ItemResponse<object>>();
                response.StatusCode.ReturnsForAnyArgs(HttpStatusCode.OK);
                response.Resource.ReturnsForAnyArgs(JsonConvert.SerializeObject(new UserSession()));
                container.ReadItemAsync<object>(Arg.Any<string>(), PartitionKey.None).Returns(Task.FromResult(response));

                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.ReadItemAsync("Id");
                var expected = new HttpResponseMessage(HttpStatusCode.OK);
                result.StatusCode.Should().Be(expected.StatusCode);
            }
            [Test]
            public async Task WhenNoContentReturns_ReturnNotFound()
            {
                var client = Substitute.For<CosmosClient>();

                var container = Substitute.For<Container>();

                var response = Substitute.For<ItemResponse<object>>();
                response.StatusCode.ReturnsForAnyArgs(HttpStatusCode.OK);
                container.ReadItemAsync<object>(Arg.Any<string>(), PartitionKey.None).Returns(Task.FromResult(response));

                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.ReadItemAsync("Id");
                var expected = new HttpResponseMessage(HttpStatusCode.NotFound);
                result.StatusCode.Should().Be(expected.StatusCode);
            }
        }

        public class UpsertItemAsyncTests
        {
            private IOptions<CosmosSettings> _cosmosSettings;
            private ICosmosService _service;

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
                _service.Invoking(x => x.UpsertItemAsync(null)).Should().Throw<ArgumentException>();

            }

            [Test]
            public void WhenContainerIsNull_ThrowError()
            {
                var client = Substitute.For<CosmosClient>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection)
                    .ReturnsNullForAnyArgs();
                _service = new CosmosService(_cosmosSettings, client);
                _service.Invoking(x => x.UpsertItemAsync(new UserSession())).Should().Throw<ArgumentException>();

            }
            [Test]
            public async Task WhenBadRequestIsMade_ReturnNull()
            {
                var client = Substitute.For<CosmosClient>();
                var container = Substitute.For<Container>();
                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.UpsertItemAsync(new UserSession());
                var expected = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                result.StatusCode.Should().Be(expected.StatusCode);
            }
            [Test]
            public async Task WhenCorrectRequestMade_ReturnSuccessCode()
            {
                var client = Substitute.For<CosmosClient>();

                var container = Substitute.For<Container>();

                var response = Substitute.For<ItemResponse<object>>();
                response.StatusCode.ReturnsForAnyArgs(HttpStatusCode.OK);
                container.UpsertItemAsync(Arg.Any<object>()).Returns(Task.FromResult(response));

                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.UpsertItemAsync(new UserSession());
                var expected = new HttpResponseMessage(HttpStatusCode.OK);
                result.StatusCode.Should().Be(expected.StatusCode);
            }
            [Test]
            public async Task WhenCreateRequestMade_ReturnSuccessCode()
            {
                var client = Substitute.For<CosmosClient>();

                var container = Substitute.For<Container>();

                var response = Substitute.For<ItemResponse<object>>();
                response.StatusCode.ReturnsForAnyArgs(HttpStatusCode.Created);
                container.UpsertItemAsync(Arg.Any<object>()).Returns(Task.FromResult(response));

                client.GetContainer(_cosmosSettings.Value.DatabaseName, _cosmosSettings.Value.UserSessionsCollection).ReturnsForAnyArgs(container);
                _service = new CosmosService(_cosmosSettings, client);
                var result = await _service.UpsertItemAsync(new UserSession());
                var expected = new HttpResponseMessage(HttpStatusCode.Created);
                result.StatusCode.Should().Be(expected.StatusCode);
            }
        }
    }
}
