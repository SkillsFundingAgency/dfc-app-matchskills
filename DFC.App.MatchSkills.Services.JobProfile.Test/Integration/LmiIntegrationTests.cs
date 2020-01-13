using DFC.App.MatchSkills.Services.JobProfile.Test.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Services.JobProfile.Test.Integration
{
    public class LmiIntegrationTests
    {
        public class SocSearchIntegrationTests
        {
            [Test]
            public void When_SocSearch_Endpoint_Hit_Return_Content()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                var results = serviceUnderTest.SocSearch(LmiHelper.SocSearchRequests.ValidSearchCriteria()).Result;
                results.Should().NotBeNull();
            }
            [Test]
            public void When_WfPredict_Endpoint_Hit_Return_Content()
            {
                
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                var results = serviceUnderTest.WFSearch(LmiHelper.WfPredictSearchRequests.ValidSearchCriteria()).Result;
                results.Should().NotBeNull();
            }
            
        }
    }
}
