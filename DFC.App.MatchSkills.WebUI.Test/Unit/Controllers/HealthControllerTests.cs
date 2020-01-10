using DFC.App.MatchSkills.WebUI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.WebUI.Test.Unit.Controllers
{
    [TestFixture]
    public class HealthControllerTests
    {
        [Test]
        public void Tdd()
        {
            // Arrange
            var controller = new HealthController(new NullLogger<HealthController>());

            // Act
            var result = controller.Ping();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
}
