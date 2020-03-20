using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

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
        public void WhenBlankSessionId_ReturnNull()
        {
            var result = _service.GetDysacJobCategories("");
            result.Should().BeNull();

        }
        [Test]
        public void WhenNullSessionId_ReturnNull()
        {
            var result = _service.GetDysacJobCategories(null);
            result.Should().BeNull();
        }

        //TODO: Uncomment when DYSAC integration allows
        //[Test]
        //public async Task WhenApiError_ReturnNull()
        //{
        //    _client = Substitute.For<IRestClient>();
        //    _client.GetAsync<DysacResults>(Arg.Any<string>(), Arg.Any<string>()).ReturnsNullForAnyArgs();
        //    _service = new DysacService(_logger, _client, _settings);
        //    var result = await _service.GetDysacJobCategories("SessionId");
        //    result.Should().BeNull();
        //}
        [Test]
        public void WhenApiSuccess_ReturnDysacResults()
        {
            var returnObject = Mapping.Mapper.Map<DysacJobCategory[]>(DysacService.TestDysacResults().JobCategories);
            _client = Substitute.For<IRestClient>();
            _client.GetAsync<DysacResults>(Arg.Any<string>()).Returns(DysacService.TestDysacResults());
            _service = new DysacService(_logger, _client, _settings);
            var result = _service.GetDysacJobCategories("SessionId");
            result[0].JobFamilyCode.Should().Be("CAM");
            result[0].JobFamilyName.Should().Be("Creative and media");
            result[0].JobFamilyUrl.Should().Be("creative-and-media");
        }
    }
}
