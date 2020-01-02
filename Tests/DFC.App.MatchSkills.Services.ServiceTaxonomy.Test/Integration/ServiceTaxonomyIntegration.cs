using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    
    class ServiceTaxonomyIntegration
    {
        private ServiceTaxonomyRepository _subjectUnderTest;    
        private string _ApiUrl;
        private string _ApiKey;
        private static HttpClient _httpClient = new HttpClient();
        [SetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _ApiUrl = config.GetSection("ServiceTaxonomySettings").GetSection("ApiUrl").Value;  
            _ApiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
           
            _subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);
        }

       
        [Test]
        public async Task When_GetAllSkills_Then_ShouldReturnSkillsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllSkills<Skill[]>(_ApiUrl + "/GetAllSkills/Execute/",_ApiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public async Task When_GetAllOccupations_Then_ShouldReturnOccupationsList()
        {
            // ACT
            var result = await _subjectUnderTest.GetAllOccupations<Occupation[]>(_ApiUrl + "/GetAllOccupations/Execute/",_ApiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [Test]
        public void When_GetAllSkillsAndMissingKey_Then_ShouldReturnException()
        {
            
            // ASSERT
            var ex = Assert.ThrowsAsync<ArgumentNullException>( ()=>  _subjectUnderTest.GetAllSkills<Skill[]>(_ApiUrl + "/GetAllSkills/Execute/",""));
            Assert.That(ex.Message,Does.Contain("Ocp-Apim-Subscription-Key must be specified"));
        }

        [Test]
        public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList()
        {
            // ACT
            var result = await _subjectUnderTest.SearchSkills<Skill[]>(_ApiUrl + "/GetAllSkills/Execute/",_ApiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [TestCase]
        public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationssList()
        {
            // ACT
            var result = await _subjectUnderTest.SearchOccupations<Occupation[]>(_ApiUrl + "/GetAllOccupations/Execute/",_ApiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        }
    }

