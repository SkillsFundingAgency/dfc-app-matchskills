using DFC.App.MatchSkills.Models;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class TestTests
    {
        [Test]
        public void PropertyTests()
        {
            var testModel = new TestModel();
            testModel.TestProperty = "test property";
            var value = testModel.TestProperty;

            value.Should().Be(testModel.TestProperty);
        }
        
    }
    
}
