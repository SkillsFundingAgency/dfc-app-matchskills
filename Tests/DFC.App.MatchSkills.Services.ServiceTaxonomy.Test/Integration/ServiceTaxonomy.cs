using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Integration
{
    [TestFixture]
    class ServiceTaxonomy
    {
        [TestFixture]
        public class ServiceTaxonomyReaderTest
        {
            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
            public async Task When_GetAllSkills_Then_ShouldReturnSkillsList(string url, string ocpApimSubscriptionKey)
            {
                // ARRANGE
                var subjectUnderTest = new ServiceTaxonomyRepository();

                // ACT
                var result = await subjectUnderTest.GetAllSkills<Skill[]>(url,ocpApimSubscriptionKey) ;

                // ASSERT
                result.Should().NotBeNull();
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllOccupations/Execute/","8ed8640b25004e26992beb9164d95139")]
            public async Task When_GetAllOccupations_Then_ShouldReturnOccupationsList(string url, string ocpApimSubscriptionKey)
            {
                // ARRANGE
                var subjectUnderTest = new ServiceTaxonomyRepository();

                // ACT
                var result = await subjectUnderTest.GetAllOccupations<Occupation[]>(url,ocpApimSubscriptionKey) ;

                // ASSERT
                result.Should().NotBeNull();
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
            public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList(string url, string ocpApimSubscriptionKey)
            {
                // ARRANGE
                var subjectUnderTest = new ServiceTaxonomyRepository();

                // ACT
                var result = await subjectUnderTest.SearchSkills<Skill[]>(url,ocpApimSubscriptionKey,"writing") ;

                // ASSERT
                result.Should().NotBeNull();
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
            public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationssList(string url, string ocpApimSubscriptionKey)
            {
                // ARRANGE
                var subjectUnderTest = new ServiceTaxonomyRepository();

                // ACT
                var result = await subjectUnderTest.SearchOccupations<Occupation[]>(url,ocpApimSubscriptionKey,"writing") ;

                // ASSERT
                result.Should().NotBeNull();
            }

        }
        
        [TestFixture]
        public class ServiceTaxonomySearcherTest
        {
          
            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
            public async Task When_SearchSkills_Then_ShouldReturnFilteredSkillsList(string url, string ocpApimSubscriptionKey)
            {
                // ARRANGE
                var subjectUnderTest = new ServiceTaxonomyRepository();

                // ACT
                var result = await subjectUnderTest.SearchSkills<Skill[]>(url,ocpApimSubscriptionKey,"writing") ;

                // ASSERT
                result.Should().NotBeNull();
            }

            [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
            public async Task When_SearchOccupations_Then_ShouldReturnFilteredOccupationssList(string url, string ocpApimSubscriptionKey)
            {
                // ARRANGE
                var subjectUnderTest = new ServiceTaxonomyRepository();

                // ACT
                var result = await subjectUnderTest.SearchOccupations<Occupation[]>(url,ocpApimSubscriptionKey,"writing") ;

                // ASSERT
                result.Should().NotBeNull();
            }

        }
    }
}
