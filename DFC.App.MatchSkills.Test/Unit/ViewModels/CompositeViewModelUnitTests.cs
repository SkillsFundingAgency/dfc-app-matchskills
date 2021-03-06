﻿using System;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.ViewModels
{
    [TestFixture]
    public class CompositeViewModelUnitTests
    {
        private IOptions<CompositeSettings> _compositeSettings;

        [SetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _compositeSettings = Options.Create(config.GetSection("CompositeSettings").Get<CompositeSettings>());
        }

        [TestFixture]
        public class PageId
        {
            [Test]
            public void When_PageIdSet_Then_ToStringReturnsPageId()
            {
                // Arrange.
                var pageId = CompositeViewModel.PageId.Home;

                // Act.
                var s = pageId.ToString();

                // Assert.
                s.Should().Be("home");
            }
            [Test]
            public void When_PageRegionSet_Then_ToStringReturnsPageRegion()
            {
                // Arrange.
                var result = CompositeViewModel.PageRegion.Body;

                // Act.
                var s = result.ToString();

                // Assert.
                s.Should().Be("body");
            }
            public void When_PageIdSet_Then_ValueReturnsPageId()
            {
                // Arrange.
                var pageId = CompositeViewModel.PageId.Home;

                // Act.
                var s = pageId.Value;

                // Assert.
                s.Should().Be("home");
            }
        }

        [TestFixture]
        public class GetElementId
        {
            [Test]
            public void When_ElementNameIsMissing_Then_ShouldThrowException()
            {
                // Arrange.
                var elementName = string.Empty;
                var instanceName = "start";
                var subject = new HomeCompositeViewModel();

                // Act.
                Action act = () => subject.GetElementId(elementName, instanceName);

                // Assert.
                act.Should().Throw<ArgumentException>().WithMessage("elementName cannot be null or empty or whitespace.*");
            }

            public void When_InstanceNameIsMissing_Then_ShouldThrowException()
            {
                // Arrange.
                var elementName = "govukStartButton";
                var instanceName = string.Empty;
                var subject = new HomeCompositeViewModel();

                // Act.
                Action act = () => subject.GetElementId(elementName, instanceName);

                // Assert.
                act.Should().Throw<ArgumentException>().WithMessage("instanceName cannot be null or empty or whitespace.*");
            }

            public void When_ValidValuesProvided_Then_ResultShouldBeCamelCased()
            {
                // Arrange.
                var elementName = "govukStartButton";
                var instanceName = "continue";
                var subject = new HomeCompositeViewModel();

                // Act.
                var result = subject.GetElementId(elementName, instanceName);

                // Assert.
                result.Should().Be("homeGovukStartButtonContinue");
            }
        }

        [TestFixture]
        public class NounForNumber
        {
            [Test]
            public void When_0_Then_NounShouldBePlural()
            {
                // Arrange.
                var number = 0;
                var itemUnderTest = new HomeCompositeViewModel();

                // Act.
                string noun = itemUnderTest.NounForNumber(number, "match", "matches");

                // Assert.
                noun.Should().Be("matches");
            }

            [Test]
            public void When_1_Then_NounShouldBeSingular()
            {
                // Arrange.
                var number = 1;
                var itemUnderTest = new HomeCompositeViewModel();

                // Act.
                string noun = itemUnderTest.NounForNumber(number, "match", "matches");

                // Assert.
                noun.Should().Be("match");
            }

            [Test]
            public void When_GreaterThan1_Then_NounShouldBePlural()
            {
                // Arrange.
                var number = 2;
                var itemUnderTest = new HomeCompositeViewModel();

                // Act.
                string noun = itemUnderTest.NounForNumber(number, "match", "matches");

                // Assert.
                noun.Should().Be("matches");
            }
        }

        [Test]
        public void When_ChildConstructed_Then_IdShouldBeSet()
        {
            // Arrange.

            // Act.
            var itemUnderTest = new HomeCompositeViewModel();

            // Assert.
            itemUnderTest.Id.Should().Be(CompositeViewModel.PageId.Home);
        }

        [Test]
        public void When_ChildConstructed_Then_PageHeadingShouldBeSet()
        {
            // Arrange.

            // Act.
            var itemUnderTest = new HomeCompositeViewModel();

            // Assert.
            itemUnderTest.PageHeading.Should().Be("Home");
        }

        [Test]
        public void When_ChildConstructed_Then_PageTitleShouldBeSet()
        {
            // Arrange.

            // Act.
            var itemUnderTest = new HomeCompositeViewModel();

            // Assert.
            itemUnderTest.PageTitle.Should().Be("Home | Discover your skills and careers");
        }
    }
}
