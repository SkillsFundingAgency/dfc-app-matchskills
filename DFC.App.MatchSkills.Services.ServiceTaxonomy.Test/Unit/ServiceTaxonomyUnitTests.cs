using System;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Unit
{
    [TestFixture]
    class ServiceTaxonomyUnitTests
    {
        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
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
            result[0].Name.Should().Be("Collect biological data");


            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
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
            result[0].Name.Should().Be("Renewable energy consultant");


            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        
        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
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


            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
        public async Task When_MockServiceSearchOccupations_Then_ShouldReturnOccupationsList(string url,string apiKey)
        { 
            // ARRANGE
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.SearchOccupations<Occupation[]>(url,apiKey,"Renewable",false);
            
            // ASSERT
            result.Should().NotBeNull(); 
            result[0].Name.Should().Be("Renewable energy consultant");

            
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post // we expected a GET request
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

     
      
      

        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
        public async Task When_GetAllSkillsForOccupation_Then_ShouldReturnSkillsList(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{skills:[" +
                                     "{\"type\": \"competency\",\"relationshipType\": \"essential\",\"skill\": \"collect biological data\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}," +
                                     "{\"type\": \"competency\",\"relationshipType\": \"essential\",\"skill\": \"collect biological info\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}," +
                                     "{\"type\": \"competency\",\"relationshipType\": \"optional\",\"skill\": \"collect samples\",\"alternativeLabels\": [\"biological data collection\", \"analysing biological records\"],\"uri\": \"aaa\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson,HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.GetAllSkillsForOccupation<Skill[]>(url,apiKey,"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197") ;
            
            // ASSERT
            result.Should().NotBeNull();
            result.Length.Should().Be(3);


            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post 
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

        [Test]
        public void When_STOccupationSkills_CreatedAllPropertoieSet()
        {
            var sut = new StOccupationSkills()
            {
                AlternativeLabels = new [] {"label1","label2"},
                LastModified =Convert.ToDateTime("1-Oct-2010"),
                Occupation = "Dentist",
                Uri = "someuri",
                Skills = new StOccupationSkills.StOsSkill[]
                {
                    new StOccupationSkills.StOsSkill()
                    {
                        AlternativeLabels = new [] {"label1","label2"},
                        LastModified =Convert.ToDateTime("1-Oct-2010"),
                        RelationshipType = "RelationshipType",
                        Skill="Skill Name",
                        SkillReusability = "SkillReusability",
                        Type="skilltype",
                        Uri="skilluri"
                    }
                }
            };

            sut.Occupation.Should().Be("Dentist");
            sut.Uri.Should().Be("someuri");
            sut.LastModified.Should().Be(Convert.ToDateTime("1-Oct-2010"));
            sut.AlternativeLabels.Should().BeEquivalentTo(new[] {"label1", "label2"});
            sut.Skills[0].Uri.Should().Be("skilluri");
            sut.Skills[0].SkillReusability.Should().Be("SkillReusability");
            sut.Skills[0].RelationshipType.Should().Be("RelationshipType");

        }

        

        class MockResult
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

    }
   
}
