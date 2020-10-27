using DFC.App.MatchSkills.Interfaces;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Service
{
    public class FileServiceTests
    {
        [Test]
        public void When_ReadAllText_ReturnTextFromFile()
        {
             var _fileService = Substitute.For<IFileService>();
             _fileService.ReadAllText("").ReturnsForAnyArgs("My File Content");
             var results = _fileService.ReadAllText("");
             
             results.Should().Be("My File Content");
            
        }
    }
}
