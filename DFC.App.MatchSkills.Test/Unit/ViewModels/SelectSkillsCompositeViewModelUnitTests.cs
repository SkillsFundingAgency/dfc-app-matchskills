using NUnit.Framework;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;

namespace DFC.App.MatchSkills.Test.Unit.ViewModels
{
    class SelectSkillsCompositeViewModelUnitTests
    {
        [Test]
        public void When_OccupationSet_Then_ToReturnOccupation()
        {
            // Arrange.
            var vm = new SelectSkillsCompositeViewModel(){Occupation = "Tester"};

            // Assert.
            vm.Occupation.Should().Be("Tester");
        }

    }
}
