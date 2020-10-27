using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Models;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Models
{
    [TestFixture]
    public class CareerMatchTests
    {
        [Test]
        public void When_Constructed_Then_AllMembersShouldBeInitialized()
        {
            // Arrange.
            var _compositeSettings = Options.Create(new CompositeSettings());
            _compositeSettings.Value.Path = "/matchskills";
            // Act.
            var sut = new CareerMatch(_compositeSettings);

            // Assert.
            sut.Should().NotBeNull();
            sut.JobSectorGrowthDescription.Should().Be(JobGrowth.Undefined);
            sut.JobProfile.Should().NotBeNull();
            sut.JobProfile.Title.Should().BeEmpty();
            sut.JobProfile.Description.Should().BeEmpty();
            sut.JobProfile.Url.Should().BeEmpty();
            sut.MatchedSkills.Should().NotBeNull();
            sut.UnMatchedSkills.Should().NotBeNull();
            sut.MatchingEssentialSkills.Should().Be(0);
            sut.MatchingOptionalSkills.Should().Be(0);
            sut.TotalOccupationEssentialSkills.Should().Be(0);
            sut.TotalOccupationOptionalSkills.Should().Be(0);
            sut.SourceSkillCount.Should().Be(0);
        }
        
        [Test]
        public void When_JobProfileUriProvided_Then_MatchSkillDetailUrlGenerated()
        {
            // Arrange
            var _compositeSettings = Options.Create(new CompositeSettings());
            _compositeSettings.Value.Path = "/matchskills";
            var cm = new CareerMatch(_compositeSettings);

            // Act.
            var url = cm.GetDetailsUrl("http://nationalcareers.service.gov.uk/jobprofile/4ade6bd5-9180-49cf-8270-6ff4730b3b2e");

            // Assert.
            url.Should().NotBeNullOrWhiteSpace();
            url.Should().Be("/matchskills/MatchDetails?id=http://nationalcareers.service.gov.uk/jobprofile/4ade6bd5-9180-49cf-8270-6ff4730b3b2e");
        }
    }
}
