using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Services.Dysac.Test.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacServiceTests
    {
        private DysacService _service;
        private ILogger<DysacService> _logger;
        private IRestClient _client;
        private IOptions<DysacSettings> _settings;

        [OneTimeSetUp]
        public void Init()
        {
            _logger = Substitute.For<ILogger<DysacService>>();
            _client = Substitute.For<IRestClient>();
            _settings = Options.Create(new DysacSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                ApiUrl = "https://this.is.anApi.org.uk"
            });
            _service = new DysacService(_logger, _client, _settings);
        }

        [Test]
        public async Task WhenBlankSessionId_ReturnNull()
        {
            var result = await _service.GetResults("");
            result.Should().BeNull();

        }
        [Test]
        public async Task WhenNullSessionId_ReturnNull()
        {
            var result = await _service.GetResults(null);
            result.Should().BeNull();
        }

        //TODO: Uncomment when DYSAC integration allows
        //[Test]
        //public async Task WhenApiError_ReturnNull()
        //{
        //    _client = Substitute.For<IRestClient>();
        //    _client.GetAsync<DysacResults>(Arg.Any<string>(), Arg.Any<string>()).ReturnsNullForAnyArgs();
        //    _service = new DysacService(_logger, _client, _settings);
        //    var result = await _service.GetResults("SessionId");
        //    result.Should().BeNull();
        //}
        [Test]
        public async Task WhenApiSuccess_ReturnDysacResults()
        {
            var returnObject = DysacService.TestDysacResults();
            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>()).Returns(returnObject);
            _service = new DysacService(_logger, _client, _settings);
            var result = await _service.GetResults("SessionId");
            result.Should().Be(returnObject);
        }
    }
}
