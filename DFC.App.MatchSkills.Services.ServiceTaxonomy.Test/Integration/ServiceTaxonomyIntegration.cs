using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Personalisation.Common.Net.RestClient;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    
    class ServiceTaxonomyIntegration
    {
        private ServiceTaxonomyRepository _subjectUnderTest;    
        private string _apiUrl;
        private string _apiKey;
        private static RestClient _restClient = new RestClient();
        private string _skillsApi;
        private string _occupationsApi;
        
        [SetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _apiUrl = config.GetSection("ServiceTaxonomySettings").GetSection("ApiUrl").Value;  
            _apiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
           
            _subjectUnderTest = new ServiceTaxonomyRepository();
            _skillsApi = _apiUrl + "/GetAllSkills/Execute/";
            _occupationsApi = _apiUrl + "/GetAllOccupations/Execute/";
        }

       
        [Test]
        public async Task When_GetAllSkills_Then_ShouldReturnSkillsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllSkills<Skill[]>(_skillsApi,_apiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_GetAllOccupations_Then_ShouldReturnOccupationsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllOccupations<Occupation[]>(_occupationsApi,_apiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public void When_GetAllSkillsAndMissingKey_Then_ShouldReturnException()
        {
            
            // ASSERT
            var ex = Assert.ThrowsAsync<ArgumentNullException>( ()=>  _subjectUnderTest.GetAllSkills<Skill[]>(_skillsApi,""));
            Assert.That(ex.Message,Does.Contain("Ocp-Apim-Subscription-Key must be specified"));
        }

        [Test]
        public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList()
        {
            // ACT
            Skill[] result = await _subjectUnderTest.SearchSkills<Skill[]>(_occupationsApi,_apiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationsList()
        {
            // ACT
            Occupation[] result = await _subjectUnderTest.SearchOccupations<Occupation[]>(_occupationsApi,_apiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

    }
}

