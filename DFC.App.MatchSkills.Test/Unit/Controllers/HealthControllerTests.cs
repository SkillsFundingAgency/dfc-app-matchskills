﻿using System.Collections.Generic;
using DFC.App.MatchSkills.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class HealthControllerTests
    {
        [Test]
        public void Ping()
        {
            // Arrange
            var controller = new HealthController(null);

            // Act
            var result = controller.Ping();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public void EndPoints()
        {
            // Arrange
            var actionDescriptorCollectionMock = new Mock<IActionDescriptorCollectionProvider>();
            actionDescriptorCollectionMock.Setup(m => m.ActionDescriptors).Returns(new ActionDescriptorCollection(new List<ActionDescriptor>(), 0));
            var controller = new HealthController(actionDescriptorCollectionMock.Object);

            // Act
            var result = controller.EndPoints();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
    }

}
