using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    [TestFixture]
    class ServiceTaxonomyIntegration
    {
        private string _ApiUrl;
        private string _ApiKey;
        private static HttpClient _httpClient = new HttpClient();
        [OneTimeSetUp]
        public void Init()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _ApiUrl = config.GetSection("ServiceTaxonomySettings").GetSection("ApiUrl").Value;  
            _ApiKey = config.GetSection("ServiceTaxonomySettings").GetSection("ApiKey").Value;  
           

        }

       
        [TestCase]
        public async Task When_GetAllSkills_Then_ShouldReturnSkillsList()
        {
            // ARRANGE
            var subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);

            // ACT
            var result = await subjectUnderTest.GetAllSkills<Skill[]>(_ApiUrl + "/GetAllSkills/Execute/",_ApiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [TestCase]
        public async Task When_GetAllOccupations_Then_ShouldReturnOccupationsList()
        {
            // ARRANGE
            var subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);

            // ACT
            var result = await subjectUnderTest.GetAllOccupations<Occupation[]>(_ApiUrl + "/GetAllOccupations/Execute/",_ApiKey) ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [TestCase]
        public void When_GetAllSkillsAndMissingKey_Then_ShouldReturnException()
        {
            // ARRANGE
            var subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);

            // ASSERT
            var ex = Assert.ThrowsAsync<ArgumentNullException>( ()=>  subjectUnderTest.GetAllSkills<Skill[]>(_ApiUrl + "/GetAllSkills/Execute/",""));
            Assert.That(ex.Message,Does.Contain("Ocp-Apim-Subscription-Key must be specified"));
        }

        [TestCase]
        public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList()
        {
            // ARRANGE
            var subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);

            // ACT
            var result = await subjectUnderTest.SearchSkills<Skill[]>(_ApiUrl + "/GetAllSkills/Execute/",_ApiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        [TestCase]
        public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationssList()
        {
            // ARRANGE
            var subjectUnderTest = new ServiceTaxonomyRepository(_httpClient);

            // ACT
            var result = await subjectUnderTest.SearchOccupations<Occupation[]>(_ApiUrl + "/GetAllSkills/Execute/",_ApiKey,"writing") ;

            // ASSERT
            result.Should().NotBeNull();
        }

        }
    }

