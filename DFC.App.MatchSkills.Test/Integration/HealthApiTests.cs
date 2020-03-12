using System.IO;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DFC.App.MatchSkills.Test.Integration
{
    [TestFixture]
    public class HealthApiTests
    {
        private TestServer _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            var projectDir = Directory.GetCurrentDirectory();
            _factory = new TestServer(
                new WebHostBuilder()
                    .UseStartup<Startup>()
                    .UseConfiguration(
                        (new ConfigurationBuilder()
                            .SetBasePath(projectDir)
                            .AddJsonFile("appsettings.json")
                            .Build()
                        )));
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task Tdd()
        {
            var result = await _client.GetAsync("/health");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
