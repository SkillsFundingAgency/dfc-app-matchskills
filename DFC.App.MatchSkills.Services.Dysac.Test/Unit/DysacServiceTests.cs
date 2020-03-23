using System;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Services.Dysac.Test.Helper;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.Threading.Tasks;
using NSubstitute.ExceptionExtensions;

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
            var result = await _service.GetDysacJobCategories("");
            result.Should().BeNull();

        }
        [Test]
        public async Task WhenNullSessionId_ReturnNull()
        {
            var result = await _service.GetDysacJobCategories(null);
            result.Should().BeNull();
        }

        [Test]
        public async Task WhenApiError_ReturnEmpty()
        {
            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>()).Throws(new Exception("Exception"));
            _service = new DysacService(_logger, _client, _settings);
            var result = await _service.GetDysacJobCategories("SessionId");
            result.Should().BeNull();
        }
        [Test]
        public async Task WhenApiSuccess_ReturnDysacResults()
        {
            var returnObject = Mapping.Mapper.Map<DysacJobCategory[]>(DysacTestData.SuccessfulApiCall().JobCategories);
            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>()).Returns(DysacTestData.SuccessfulApiCall());
            _service = new DysacService(_logger, _client, _settings);
            var result = await _service.GetDysacJobCategories("SessionId");
            result[0].JobFamilyCode.Should().Be("CAM");
            result[0].JobFamilyName.Should().Be("Creative and media");
            result[0].JobFamilyUrl.Should().Be("creative-and-media");
        }

    }
}
