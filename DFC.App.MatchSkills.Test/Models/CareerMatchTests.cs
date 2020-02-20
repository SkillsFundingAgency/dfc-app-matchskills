using DFC.App.MatchSkills.Models;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
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

            // Act.
            var sut = new CareerMatch();

            // Assert.
            sut.Should().NotBeNull();
            sut.JobSectorGrowthDescription.Should().BeEmpty();
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
        public void When_SkillsExist_Then_MatchPercentageShouldBeCalculated()
        {
            // Arrange.
            var cm = new CareerMatch()
            {
                JobSectorGrowthDescription = "Increasing",
            };
            cm.JobProfile.Title = "Job Title of First Match";
            cm.JobProfile.Description = "Here is a description of the job profile.";
            cm.JobProfile.Url = "http://jobprofile";
            cm.MatchedSkills.Add(new Skill("fm1", "First matched skill", SkillType.Competency));
            cm.MatchedSkills.Add(new Skill("fm2", "Second  matched skill", SkillType.Competency));
            cm.MatchedSkills.Add(new Skill("fm3", "Third matched skill", SkillType.Competency));
            cm.UnMatchedSkills.Add(new Skill("um1", "First unmatched skill", SkillType.Competency));
            cm.SourceSkillCount = 4;
            cm.MatchingEssentialSkills = 1;
            cm.MatchingOptionalSkills = 2;
            cm.TotalOccupationEssentialSkills = 10;
            cm.TotalOccupationOptionalSkills = 3;

            // Act.
            var result = cm.MatchStrengthPercentage;

            // Assert.
            result.Should().Be(75);
        }
    }
}
