using DFC.Personalisation.Common.Net.RestClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DFC.App.MatchSkills.Services.JobProfile.Models;
using DFC.App.MatchSkills.Services.JobProfile.Test.Helpers;
using FluentAssertions;

namespace DFC.App.MatchSkills.Application.Test.Unit
{

    public class LMIUnitTests
    {
        private const string ApiUrl = "http://api.lmiforall.org.uk/api/v1/";
        private const string SocSearchPath = "soc/search";
        private const string WfPredictSearchPath = "wf/predict";
        private readonly RestClient client = new RestClient();

        public class SocSearchTests
        {
            [Test]
            public void When_Criteria_Is_Null_Return_Argument_Exception()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                serviceUnderTest.Awaiting(x => x.SocSearch(null)).ShouldThrow<ArgumentException>()
                    .WithMessage("SearchCriteria cannot be null. (Parameter 'criteria')");
            }

            [Test]
            public void When_Criteria_Is_Empty_Return_Argument_Exception()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                serviceUnderTest.Awaiting(x => x.SocSearch(new SocSearchCriteria{SearchCriteria = string.Empty})).ShouldThrow<ArgumentException>()
                    .WithMessage("SearchCriteria cannot be empty. (Parameter 'criteria')");
            }

            [Test]
            public void When_BadRequest_Return_HttpRequestException()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient(HttpClientMockFactory.Post_BadRequest_Mock().Object);
                serviceUnderTest.Awaiting(x => x.SocSearch(LmiHelper.SocSearchRequests.ValidSearchCriteria())).ShouldThrow<HttpRequestException>()
                    .WithMessage("Response status code does not indicate success: 400 (Bad Request).");
            }
        }

        public class WFSearchTest
        {
            [Test]
            public void When_Request_Is_Null_Throw_ArgumentException()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                serviceUnderTest.Awaiting(x => x.WFSearch(null)).ShouldThrow<ArgumentException>()
                    .WithMessage("Request cannot be null. (Parameter 'request')");
            }
            
            [Test]
            public void When_SocCode_Is_Zero_Throw_Argument_Exception()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                serviceUnderTest.Awaiting(x => x.WFSearch(new WorkingFuturesRequest{ SocCode = 0})).ShouldThrow<ArgumentException>()
                    .WithMessage("SocCode cannot be zero. (Parameter 'socCode')");
            }

            [Test]
            public void When_SocCode_Is_A_Negative_Int_Throw_Argument_Exception()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient();
                serviceUnderTest.Awaiting(x => x.WFSearch(new WorkingFuturesRequest{ SocCode = -1})).ShouldThrow<ArgumentException>()
                    .WithMessage("SocCode cannot be less than zero. (Parameter 'socCode')");
            }
            [Test]
            public void When_BadRequest_Return_HttpRequestException()
            {
                var serviceUnderTest = LmiHelper.LmiService_RestClient(HttpClientMockFactory.Post_BadRequest_Mock().Object);
                serviceUnderTest.Awaiting(x => x.WFSearch(LmiHelper.WfPredictSearchRequests.ValidSearchCriteria())).ShouldThrow<HttpRequestException>()
                    .WithMessage("Response status code does not indicate success: 400 (Bad Request).");
            }
        
        }


        [Test]
        public void Call_SOC_Get_Value_Returned()
        {
            var result = client.Get<IEnumerable<SocSearchResults>>($"{ApiUrl}{SocSearchPath}?q=Developer").Result;
            var socId = result.Select(x => x.Soc).FirstOrDefault();

            var prediction = client.Get<WorkingFuturesSearchResults>($"{ApiUrl}{WfPredictSearchPath}?soc={socId}").Result;
            Dictionary<string, decimal> growth = new Dictionary<string, decimal>();
            Console.WriteLine("Year span\t" + "Percentage Change");
            for(var i = 0; i < prediction.PredictedEmployment.Count; i++)
            {
                if (i != prediction.PredictedEmployment.Count - 1)
                {
                    var pastYearValue = prediction.PredictedEmployment[i].Employment;
                    var futureYearValue = prediction.PredictedEmployment[i + 1].Employment;

                    var yearSpan = $"{prediction.PredictedEmployment[i].Year}-{prediction.PredictedEmployment[i + 1].Year}";
                    var percentageChange = GrowthCalc(pastYearValue, futureYearValue);
                    growth.Add(yearSpan, percentageChange);
                    Console.WriteLine($"{yearSpan}\t{percentageChange}");
                }
            }
        }

        public decimal GrowthCalc(long pastYear, long futureYear)
        {
            if (pastYear == 0)
                throw new InvalidOperationException();

            var change = futureYear - pastYear;
            var percentage =  ((decimal)change / pastYear) * 100;
            return Math.Truncate(percentage * 1000m) / 1000m;

        }
    }
}
