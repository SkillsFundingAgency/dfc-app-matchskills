using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.LMI.Services;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

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
                    ApiUrl = "Url"
                });
                _restClient = Substitute.For<RestClient>();
            }


            [Test]
            public async Task IfMatchesIsNullOrEmpty_ReturnMatches()
            {
                var serviceUnderTest = new LmiService(_restClient, _settings);
                var matches = new List<OccupationMatch>();

                var result = await serviceUnderTest.GetPredictionsForGetOccupationMatches(matches);

                result.Should().Equal(matches);
            }
        }

    }
}
