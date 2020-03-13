﻿using DFC.App.MatchSkills.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class HealthControllerTests
    {
        [Test]
        public void Tdd()
        {
            // Arrange
            var controller = new HealthController(null);

            // Act
            var result = controller.Ping();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }
    }
    
}
