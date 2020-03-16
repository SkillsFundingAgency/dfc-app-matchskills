using DFC.App.MatchSkills.ViewModels;
using NUnit.Framework;
using FluentAssertions;

namespace DFC.App.MatchSkills.Test.Unit.ViewModels
{
    [TestFixture]
    public class EndPointsViewModelTests
    {
        [Test]
        public void When_Constructed_Then_Initialised()
        {
            // Arrange.


            // Act.
            var model = new EndPointsViewModel();

            // Assert.
            model.Should().NotBeNull();
            model.EndPoints.Should().NotBeNull();
            model.EndPoints.Should().BeEmpty();
        }
    }
}
