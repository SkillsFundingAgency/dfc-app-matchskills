using DFC.App.MatchSkills.Services.Dysac.Test.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Http;
using DFC.App.MatchSkills.Application.Dysac.Models;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacSessionUnitTests
    {

        public class CreateNewSessionTests
        {
           
            [Test]
            public void If_Bad_Request_Return_Null()
            {
                var serviceUnderTest =
                    SessionHelper.CreateNewDysacSession(HttpClientMockFactory.Post_BadRequest_Mock().Object);
                serviceUnderTest.Awaiting(x => x.CreateNewSession(AssessmentTypes.Short)).ShouldThrow<HttpRequestException>()
                    .WithMessage("Response status code does not indicate success: 400 (Bad Request).");

            }
            
            [Test]
            public void If_Valid_Request_Return_Response()
            {
                var serviceUnderTest =
                    SessionHelper.CreateNewDysacSession(HttpClientMockFactory.Post_Successful_Mock().Object);

                var results = serviceUnderTest.CreateNewSession(AssessmentTypes.Short).Result;
                results.SessionId.Should().Be("0fcf719b-2aea-4af4-a2ba-6e73ccd5105b");
                results.QuestionNumber.Should().Be(0);

            }

        }




    }
}
