using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Services.Dysac.Test.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacSessionUnitTests
    {

        public class CreateNewSessionTests
        {
            [Test]
            public void If_Null_Assessment_Type_Argument_Then_Fail()
            {
                var serviceUnderTest = SessionHelper.CreateNewDysacSession();
                serviceUnderTest.Awaiting(x => x.CreateNewSession(null)).ShouldThrow<ArgumentException>()
                    .WithMessage("Null or empty assessment type passed");
            }

            [Test]
            public void If_Blank_Assessment_Type_Argument_Then_Fail()
            {
                var serviceUnderTest = SessionHelper.CreateNewDysacSession();
                serviceUnderTest.Awaiting(x => x.CreateNewSession(string.Empty)).ShouldThrow<ArgumentException>()
                    .WithMessage("Null or empty assessment type passed");
            }

            [Test]
            public void If_Enum_Undefined_Throw_Exception()
            {
                var serviceUnderTest = SessionHelper.CreateNewDysacSession();
                serviceUnderTest.Awaiting(x => x.CreateNewSession("incorrectValue")).ShouldThrow<ArgumentException>()
                    .WithMessage("Invalid value passed");

            }

            [Test]
            public void If_Bad_Request_Return_Null()
            {
                var serviceUnderTest =
                    SessionHelper.CreateNewDysacSession(HttpClientMockFactory.Post_BadRequest_Mock().Object);
                serviceUnderTest.CreateNewSession("short").Result.Should().BeNull();

            }
            
            [Test]
            public void If_Valid_Request_Return_Response()
            {
                var serviceUnderTest =
                    SessionHelper.CreateNewDysacSession(HttpClientMockFactory.Post_Successful_Mock().Object);
                var results = serviceUnderTest.CreateNewSession("short").Result;
                results.SessionId.Should().Be("0fcf719b-2aea-4af4-a2ba-6e73ccd5105b");
                results.QuestionNumber.Should().Be(0);

            }

        }




    }
}
