using DFC.App.MatchSkills.Application.LMI.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DFC.App.MatchSkills.Application.Test.Unit.Helpers
{
    public class LmiHelperTest
    {
        [Test]
        public void When_LastCheckedDate_IsOutOfDate_ReturnFalse()
        {
            var date = DateTimeOffset.Now.AddDays(366);

            var result = LmiHelper.IsOutOfDate(date, 365);

            result.Should().BeFalse();
        }
    }
}
