using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Unit
{
    class ServiceTaxonomyUnitTests
    {
        [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
        public async Task When_MockServiceGetSkills_Then_ShouldReturnSkillsObject(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{skills:[{\"skillType\": \"competency\",\"skill\": \"collect biological data\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson,HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.GetAllSkills<Skill[]>(url,apiKey) ;
            
           
            // ASSERT
            result.Should().NotBeNull(); 
            result[0].Name.Should().Be("collect biological data");

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri(url);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllOccupations/Execute/","8ed8640b25004e26992beb9164d95139")]
        public async Task When_MockServiceGetOccupations_Then_ShouldReturnOccupationsObject(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.GetAllOccupations<Occupation[]>(url,apiKey) ;
            
            // ASSERT
            result.Should().NotBeNull(); 
            result[0].Name.Should().Be("renewable energy consultant");

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri(url);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
        public async Task When_MockServiceSearchSkills_Then_ShouldReturnFilteredSkillsList(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{skills:[{\"skillType\": \"competency\",\"skill\": \"collect biological data\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"},{\"skillType\": \"competency\",\"skill\": \"collect biological info\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"},{\"skillType\": \"competency\",\"skill\": \"collect samples\",\"alternativeLabels\": [\"biological data collection\", \"analysing biological records\"],\"uri\": \"aaa\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson,HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.SearchSkills<Skill[]>(url,apiKey,"biological") ;
            
            // ASSERT
            result.Should().NotBeNull();
            result.Length.Should().Be(2);

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri(url);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllOccupations/Execute/","8ed8640b25004e26992beb9164d95139")]
        public async Task When_MockServiceSearchOccupations_Then_ShouldReturnOccupationsList(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"},{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable paper consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"},{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"water consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.SearchOccupations<Occupation[]>(url,apiKey,"renewable");
            
            // ASSERT
            result.Should().NotBeNull(); 
            result[0].Name.Should().Be("renewable energy consultant");

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri(url);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }


        public  Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn="{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn=HttpStatusCode.OK)
        {
            var handlerMock =  new Mock<HttpMessageHandler>(MockBehavior.Loose);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )

                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusToReturn,
                    Content = new StringContent(contentToReturn)
                })
                .Verifiable();
            return handlerMock;
        }

    }
   
}
