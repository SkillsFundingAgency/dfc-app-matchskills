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
            public void When_InitiateDysacWithNullSession_ReturnOK()
            {
                var serviceUnderTest = SessionHelper.CreateNewDysacSession(HttpClientMockFactory.Post_Successful_Mock().Object);

                var results = serviceUnderTest.InitiateDysac().Result;
                results.ResponseCode.Should().Be(DysacReturnCode.Ok);
                
            }

        }




    }
}
