using Dfc.Session;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Services.Dysac.Test.Helper;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacServiceTests
    {
        private DysacService _service;
        private ILogger<DysacService> _logger;
        private IRestClient _client;
        private IOptions<DysacSettings> _settings;
        private IOptions<OldDysacSettings> _oldDysacSettings;
        private ISessionClient _sessionClient;

        [OneTimeSetUp]
        public void Init()
        {
            _logger = Substitute.For<ILogger<DysacService>>();
            _client = Substitute.For<IRestClient>();
            _sessionClient = Substitute.For<ISessionClient>();
            _settings = Options.Create(new DysacSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                ApiUrl = "https://this.is.anApi.org.uk",
                ApiVersion = "v1",
                DysacReturnUrl = "SomeURL",
                DysacSaveUrl = "SaveURL",
                DysacUrl = "DysacURL"
            });
            _oldDysacSettings = Options.Create(new OldDysacSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                AssessmentApiUrl = "https://this.is.anApi.org.uk",
                DysacResultsUrl = "https://this.is.anApi.org.uk",
            });

            _service = new DysacService(_logger, _client, _settings, _oldDysacSettings, _sessionClient);

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
            var oldDysacSettings = new OldDysacSettings()
            {
                ApiKey = "key",
                AssessmentApiUrl = "Url",
                DysacResultsUrl = "url"
            };
            var key = oldDysacSettings.ApiKey;
            var assessment = oldDysacSettings.AssessmentApiUrl;
            var dysac = oldDysacSettings.DysacResultsUrl;

            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).Throws(new Exception("Exception"));
            _service = new DysacService(_logger, _client, _settings, Options.Create(oldDysacSettings), _sessionClient);
            var result = await _service.GetDysacJobCategories("SessionId");
            result.Should().BeNull();
            key.Should().Be(oldDysacSettings.ApiKey);
            assessment.Should().Be(oldDysacSettings.AssessmentApiUrl);
            dysac.Should().Be(oldDysacSettings.DysacResultsUrl);
        }
        [Test]
        public async Task WhenNullResponse_ReturnEmpty()
        {
            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>()).ReturnsNullForAnyArgs();
            _service = new DysacService(_logger, _client, _settings, _oldDysacSettings, _sessionClient);
            var result = await _service.GetDysacJobCategories("SessionId");
            result.Should().BeEmpty();
        }
        [Test]
        public async Task WhenApiSuccess_ReturnDysacResults()
        {
            var response = new DysacServiceResponse();
            response.ResponseMessage = "message";
            response.ResponseMessage = "message";
            var returnObject = Mapping.Mapper.Map<DysacJobCategory[]>(DysacTestData.SuccessfulApiCall().JobCategories);
            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).Returns(DysacTestData.SuccessfulApiCall());
            _service = new DysacService(_logger, _client, _settings, _oldDysacSettings, _sessionClient);
            var result = await _service.GetDysacJobCategories("SessionId");
            result[0].JobFamilyCode.Should().Be("CAM");
            result[0].JobFamilyName.Should().Be("Creative and media");
            result[0].JobFamilyUrl.Should().Be("creative-and-media");
        }
        

    }
}
