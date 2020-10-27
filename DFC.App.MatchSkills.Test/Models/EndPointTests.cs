using DFC.App.MatchSkills.Models;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Models
{
    [TestFixture]
    public class EndPointTests
    {
        [Test]
        public void When_Constructed_Then_NotInitialised()
        {
            // Arrange.


            // Act.
            var model = new EndPoint();

            // Assert.
            model.Should().NotBeNull();
            model.Methods.Should().BeNullOrEmpty();
            model.Controller.Should().BeNullOrEmpty();
            model.Action.Should().BeNullOrEmpty();
        }
    }
}
