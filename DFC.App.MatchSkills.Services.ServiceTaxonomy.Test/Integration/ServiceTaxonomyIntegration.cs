using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    [TestFixture]
    [Ignore("Settings Issue")]
    class ServiceTaxonomyIntegration
    {
        // [Ignore("Need to figure out how we are going to handle the settings")]
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

        [TestCase("http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197")]
        public async Task When_GetAllSkillsForOccupation_Then_ShouldReturnSkillsList(string occupation)
        {
            // ACT
            var result = await _subjectUnderTest.GetAllSkillsForOccupation<Skill[]>(_settings.ApiUrl,_settings.ApiKey,occupation) ;

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

        [Test]
        public async Task When_FindOccupationsForSkills_Then_ShouldReturnOccupationsList()
        {
            // Arrange
            string[] skillIds = new string[]
            {
                "http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99",  // "lift heavy weights
                "http://data.europa.eu/esco/skill/28cb374e-6261-4133-8371-f9a5470145da",  // "operate forklift"
            };
            int minimumMatchingSkills = 1;

            // Act
            var result = await _subjectUnderTest.FindOccupationsForSkills(_settings.ApiUrl, _settings.ApiKey, skillIds, minimumMatchingSkills);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(24);
        }

    }
}

