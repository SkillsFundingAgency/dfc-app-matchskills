using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFC.App.MatchSkills.Application.Session.Helpers;
using FluentAssertions;
using HashidsNet;
using Moq;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Application.Test.Unit.Helpers
{
    public class SessionIdHelperTests
    {
        public class GenerateSessionIdTests
        {
            [Test]
            public void WhenGenerateSessionIdCalledWithoutSalt_ValueShouldNotBeNull()
            {
                var sessionId = SessionIdHelper.GenerateSessionId(null, DateTime.UtcNow);
                sessionId.Should().NotBeNullOrWhiteSpace();
                sessionId.Should().NotBeNullOrEmpty();

            }
            [Test]
            public void WhenGenerateSessionIdCalledWithoutSaltAndDate_ValueShouldNotBeNull()
            {
                var sessionId = SessionIdHelper.GenerateSessionId(null);
                sessionId.Should().NotBeNullOrWhiteSpace();
                sessionId.Should().NotBeNullOrEmpty();

            }
            [Test]
            public void WhenGenerateSessionIdCalledWithSaltAndWithoutDate_ValueShouldNotBeNull()
            {
                var sessionId = SessionIdHelper.GenerateSessionId("Salt");
                sessionId.Should().NotBeNullOrWhiteSpace();
                sessionId.Should().NotBeNullOrEmpty();

            }
            [Test]
            public void WhenGenerateSessionIdCalledWithSaltIsWhitespace_ValueShouldNotBeNull()
            {
                var sessionId = SessionIdHelper.GenerateSessionId("", DateTime.UtcNow);
                sessionId.Should().NotBeNullOrWhiteSpace();
                sessionId.Should().NotBeNullOrEmpty();
            }
            [Test]
            public void EncodedValueShouldReturnSameValueWhenDecoded()
            {
                
                string Alphabet = "acefghjkmnrstwxyz23456789";
                var salt = "BatteryHorseStapleCorrect";
                var sessionId = SessionIdHelper.GenerateSessionId(salt, DateTime.UtcNow);
                var hashids = new Hashids(salt, 4, Alphabet);
                var digits = hashids.DecodeLong(sessionId).First();

                var decode = SessionIdHelper.Decode(salt, sessionId);
                decode.Should().Be(digits.ToString());
                
            }
        }

        public class PartitionKeyGeneratorTests
        {
            [Test]
            public void WhenUserSession_ValueReturned()
            {
                string sessionId = "nkzr76jwwmz4yj";
                var value = PartitionKeyHelper.UserSession(sessionId);
                value.Should().NotBeNullOrWhiteSpace();
            }

            [Test]
            public void WhenSessionId_IsWhitespace_ReturnCorrectValue()
            {
                string sessionId = "";
                var value = PartitionKeyHelper.UserSession(sessionId);
                value.Should().NotBeNullOrWhiteSpace();
            }
        }
    }
}
