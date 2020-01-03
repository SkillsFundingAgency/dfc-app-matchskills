using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

namespace DFC.App.MatchSkills.WebUI.Test.Integration
{
    [TestFixture]
    public class HealthApiTests
    {
        private ApiWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new ApiWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task tdd()
        {
            var result = await _client.GetAsync("/health");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
