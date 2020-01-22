using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    [TestFixture]
    [Ignore("Need to figure out how we are going to handle the settings")]
    class ServiceTaxonomyIntegration
    {
        private ServiceTaxonomyRepository _subjectUnderTest;
        private static RestClient _restClient = new RestClient();
        private ServiceTaxonomySettings _settings =  new ServiceTaxonomySettings();

       
        [SetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            _settings.ApiUrl = config.GetSection("ServiceTaxonomySettings").GetSection("ApiUrl").Value;
            _settings.ApiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
           
            _subjectUnderTest = new ServiceTaxonomyRepository();
        }

       
        [Test]
        public async Task When_GetAllSkills_Then_ShouldReturnSkillsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllSkills<Skill[]>(_settings.ApiUrl,_settings.ApiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_GetAllOccupations_Then_ShouldReturnOccupationsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllOccupations<Occupation[]>(_settings.ApiUrl,_settings.ApiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }


        [Test]
        public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList()
        {
            // ACT
            Skill[] result = await _subjectUnderTest.SearchSkills<Skill[]>(_settings.ApiUrl,_settings.ApiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationsList()
        {
            // ACT
            Occupation[] result = await _subjectUnderTest.SearchOccupations<Occupation[]>(_settings.ApiUrl,_settings.ApiKey,"writing",false) ;

            // ASSERT
            result.Should().NotBeNull();
        }

    }
}

