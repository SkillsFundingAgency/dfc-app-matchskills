using System;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.LMI.Services;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Test.Unit.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.Test.Unit.Services
{
    public class LmiServiceUnitTests
    {

        public class GetPredictionsForGetOccupationMatchesTests
        {
            private IOptions<LmiSettings> _settings;
            private RestClient _restClient;

            [OneTimeSetUp]
            public void Init()
            {
                _settings = Options.Create(new LmiSettings()
                {
                    ApiUrl = "http://thisisarealuri.com"
                });
                var mockHandler = LmiHelpers.GetMockMessageHandler(LmiHelpers.SuccessfulLmiCall());
                _restClient = new RestClient(mockHandler.Object);
            }


            [Test]
            public async Task IfMatchesIsNull_ReturnMatches()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings);

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(null);

                result.Should().BeNull();
            }
            [Test]
            public async Task IfMatchesIsEmpty_ReturnMatches()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>();

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.Should().Equal(matches);
            }
            [Test]
            public async Task IfSocCodeIsZero_ReturnMatchesWithoutGrowth()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 0
                    }
                };

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);
            }
            [Test]
            public async Task IfSocCodeIsLessThanZero_ReturnMatchesWithoutGrowth()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = -1
                    }
                };

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);
            }
            [Test]
            public async Task IfUnsuccessfulCall_ReturnMatchesWithoutGrowth()
            {
                var mockHandler = LmiHelpers.GetMockMessageHandler(string.Empty, HttpStatusCode.BadRequest);
                _restClient = new RestClient(mockHandler.Object);
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 2815
                    }
                };

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Undefined);
            }
            [Test]
            public async Task IfSuccessfulCall_ReturnMatchesWithGrowth()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 2815
                    }
                };

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.FirstOrDefault().JobGrowth.Should().Be(JobGrowth.Increasing);


            }
            [Test]
            public async Task IfSuccessfulCall_WithNullContentReturnMatchesWithoutGrowth()
            {
                var wfResult = JsonConvert.SerializeObject(new WfPredictionResult());
                var mockHandler = LmiHelpers.GetMockMessageHandler(wfResult, HttpStatusCode.OK);
                _restClient = new RestClient(mockHandler.Object);
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>
                {
                    new OccupationMatch
                    {
                        SocCode = 2815
                    }
                };

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

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
