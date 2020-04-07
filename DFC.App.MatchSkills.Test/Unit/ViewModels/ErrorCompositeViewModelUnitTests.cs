using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.ViewModels
{
    [TestFixture]
    public class ErrorCompositeViewModelUnitTests
    {
        [TestFixture]
        public class FormatRTACode
        {
            [Test]
            public void When_NotInitialised_Then_EmptyString()
            {
                // Arrange.
                var vm = new ErrorCompositeViewModel();

                // Act.
                vm.RTACode = null;

                // Assert.
                vm.RTACode.Should().Be(string.Empty);
            }

            [Test]
            public void When_Null_Then_EmptyString()
            {
                // Arrange.
                var vm = new ErrorCompositeViewModel();

                // Act.
                vm.RTACode = null;

                // Assert.
                vm.RTACode.Should().Be(string.Empty);
            }

            [Test]
            public void When_Whitespace_Then_EmptyString()
            {
                // Arrange.
                var vm = new ErrorCompositeViewModel();

                // Act.
                vm.RTACode = "     ";

                // Assert.
                vm.RTACode.Should().Be(string.Empty);
            }

            [Test]
            public void When_NonEmptyFewerThanFourCharacters_Then_ReturnCharacters()
            {
                // Arrange.
                var vm = new ErrorCompositeViewModel();

                // Act.
                vm.RTACode = "123";

                // Assert.
                vm.RTACode.Should().Be("123");
            }

            [Test]
            public void When_NonEmptyMoreThan4Characters_Then_ReturnCharactersGroupedInFours()
            {
                // Arrange.
                var vm = new ErrorCompositeViewModel();

                // Act.
                vm.RTACode = "123456789";

                // Assert.
                vm.RTACode.Should().Be("1234 5678 9");
            }
        }
    }
}
