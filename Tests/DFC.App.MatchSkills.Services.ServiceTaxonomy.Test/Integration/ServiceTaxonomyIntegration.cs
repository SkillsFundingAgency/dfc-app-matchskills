using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    
    class ServiceTaxonomyIntegration
    {
        private ServiceTaxonomyRepository _subjectUnderTest;    
        private string _apiUrl;
        private string _apiKey;
        private static HttpClient _httpClient = new HttpClient();
        [SetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _apiUrl = config.GetSection("ServiceTaxonomySettings").GetSection("ApiUrl").Value;  
            _apiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
           
            _subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);
        }

       
        [Test]
        public async Task When_GetAllSkills_Then_ShouldReturnSkillsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllSkills<Skill[]>(_apiUrl + "/GetAllSkills/Execute/",_apiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_GetAllOccupations_Then_ShouldReturnOccupationsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllOccupations<Occupation[]>(_apiUrl + "/GetAllOccupations/Execute/",_apiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public void When_GetAllSkillsAndMissingKey_Then_ShouldReturnException()
        {
            
            // ASSERT
            var ex = Assert.ThrowsAsync<ArgumentNullException>( ()=>  _subjectUnderTest.GetAllSkills<Skill[]>(_apiUrl + "/GetAllSkills/Execute/",""));
            Assert.That(ex.Message,Does.Contain("Ocp-Apim-Subscription-Key must be specified"));
        }

        [Test]
        public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList()
        {
            // ACT
            var result = await _subjectUnderTest.SearchSkills<Skill[]>(_apiUrl + "/GetAllSkills/Execute/",_apiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [TestCase]
        public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationsList()
        {
            // ACT
            var result = await _subjectUnderTest.SearchOccupations<Occupation[]>(_apiUrl + "/GetAllOccupations/Execute/",_apiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        }
    }

