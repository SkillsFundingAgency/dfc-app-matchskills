using System;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.LMI.Services;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Test.Unit.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Cosmos.Services;
using DFC.App.MatchSkills.Application.LMI.Interfaces;
using Microsoft.Azure.Cosmos;
using NSubstitute;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class LmiServiceUnitTests
    {

        public class GetPredictionsForGetOccupationMatchesTests
        {
            private IOptions<LmiSettings> _settings;
            private RestClient _restClient;
            private ICosmosService _cosmosService;

            [OneTimeSetUp]
            public void Init()
            {
                _settings = Options.Create(new LmiSettings()
                {
                    ApiUrl = "http://thisisarealuri.com"
                });
                var cachedLmiDataModel = new CachedLmiData
                {
                    SocCode = 2815.ToString(),
                    JobGrowth = JobGrowth.Increasing,
                    DateWritten = DateTimeOffset.Now
                };
                var cachedLmiData = new StringContent(JsonConvert.SerializeObject(cachedLmiDataModel));
                var mockHandler = LmiHelpers.GetMockMessageHandler(LmiHelpers.SuccessfulLmiCall());
                _restClient = new RestClient(mockHandler.Object);
                _cosmosService = Substitute.For<ICosmosService>();
                _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
                    .Returns(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = cachedLmiData
                    });
                _cosmosService.CreateItemAsync(Arg.Any<object>(), Arg.Any<CosmosCollection>())
                    .Returns(new HttpResponseMessage(HttpStatusCode.OK));
                _cosmosService.UpsertItemAsync(Arg.Any<object>(), Arg.Any<CosmosCollection>())
                    .Returns(new HttpResponseMessage(HttpStatusCode.OK));

            }


            [Test]
            public void IfMatchesIsNull_ReturnMatches()
            {
                var serviceUnderTest = new LmiService(_settings, _cosmosService);

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(null);

                result.Should().BeNull();
            }
            [Test]
            public void IfMatchesIsEmpty_ReturnMatches()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings, _cosmosService);
                var matches = new List<OccupationMatch>();

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.Should().Equal(matches);
            }
            [Test]
            public void IfSocCodeIsZero_ReturnMatchesWithoutGrowth()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings, _cosmosService);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 0
                    }
                };

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);
            }
            [Test]
            public void IfSocCodeIsLessThanZero_ReturnMatchesWithoutGrowth()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings, _cosmosService);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = -1
                    }
                };

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);
            }
            [Test]
            public void IfUnsuccessfulCall_ReturnMatchesWithoutGrowth()
            {
                var mockHandler = LmiHelpers.GetMockMessageHandler(string.Empty, HttpStatusCode.BadRequest);
                _restClient = new RestClient(mockHandler.Object);
                var serviceUnderTest = new LmiService(_restClient, _settings, _cosmosService);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 2815
                    }
                };

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);
            }
            [Test]
            public void IfSuccessfulCall_ReturnMatchesWithGrowth()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings, _cosmosService);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 2815
                    }
                };

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Increasing);


            }
            [Test]
            public void IfSuccessfulCall_WithNullContentReturnMatchesWithoutGrowth()
            {
                var wfResult = JsonConvert.SerializeObject(new WfPredictionResult());
                var mockHandler = LmiHelpers.GetMockMessageHandler(wfResult, HttpStatusCode.OK);
                _restClient = new RestClient(mockHandler.Object);
                var serviceUnderTest = new LmiService(_restClient, _settings, _cosmosService);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 2815
                    }
                };

                var result = serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);


            }
        }

        [Test]
        public void ModelAssigning()
        {
            WfPredictionResult result = new WfPredictionResult
            {
                Breakdown = "breakdown",
                Note = "note",
                PredictedEmployment = new PredictedEmployment[1],
                Soc = 12
            };
            var breakdown = result.Breakdown;
            var note = result.Note;
            var predictedEmployment = result.PredictedEmployment;
            var soc = result.Soc;
            
            var breakdownModel = new Breakdown
            {
                Code = Region.London,
                Employment = 2,
                Name = "Name",
                Note = "Note"
            };

            var code = breakdownModel.Code;
            var employment = breakdownModel.Note;
            var name = breakdownModel.Name;
            var noteBreakdown = breakdownModel.Note;

        }
    }
}
