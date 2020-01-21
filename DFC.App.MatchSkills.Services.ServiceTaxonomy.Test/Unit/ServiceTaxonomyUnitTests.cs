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
            result[0].Name.Should().Be("collect biological data");


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
            result[0].Name.Should().Be("renewable energy consultant");


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
            var result = await subjectUnderTest.SearchOccupations<Occupation[]>(url,apiKey,"renewable",false);
            
            // ASSERT
            result.Should().NotBeNull(); 
            result[0].Name.Should().Be("renewable energy consultant");

            
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post // we expected a GET request
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public void WhenStSkillsPropertyUpdated_Then_ShouldStoreCorrectly()
        {
            var sut = new StSkills()
            {
                Skills = new []
                {
                    new StSkill
                    {
                        Skill = "skill1", AlternativeLabels = new[] {"skill1alt", "skill1alt2"}, SkillType = "type",
                        Uri = "uri"
                    },
                    new StSkill
                    {
                        Skill = "skill1", AlternativeLabels = new[] {"skill2alt", "skill2alt2"}, SkillType = "type",
                        Uri = "uri"
                    }
                }
            };

            sut.Skills.Length.Should().Be(2);

        }

        [Test]
        public void WhenStOccupationsPropertyUpdated_Then_ShouldStoreCorrectly()
        {
            var sut = new StOccupations()
            {
                Occupations = new StOccupation[]
                {
                    new StOccupation
                    {
                        Occupation = "Occ1", AlternativeLabels = new[] {"Alt1,Alt2"}, Uri = "Uri", LastModified = DateTime.Now
                    },
                    new StOccupation
                    {
                            Occupation = "Occ2",AlternativeLabels =new []{ "Alt1,Alt2"},Uri = "Uri",LastModified = DateTime.Now
                    }
                }
            };
            sut.Occupations.Length.Should().Be(2);
        }

        [Test]
        public void WhenStSkillsPropertyUpdated_Then_ShouldStoreCorrectly()
        {
            var sut = new StSkills()
            {
                Skills = new StSkill[]
                {
                    new StSkill
                    {
                        Skill = "Skill1", SkillType = "type"
                    }                }
            };
            sut.Skills.Length.Should().Be(1);
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk")]
        public async Task When_MockServiceGet_Then_ShouldReturnObject(string url)
        {
            //ARRANGE
            var handlerMock = GetMockMessageHandler();
            var _subjectUnderTest = new RestClient(handlerMock.Object);
            
            // ACT
            var result = await _subjectUnderTest.GetAsync<MockResult>(url);

            // ASSERT
            result.Should().NotBeNull(); // this is fluent assertions here...
            result.Id.Should().Be(1);

        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk")]
        public async Task When_MockServicePost_Then_ShouldReturnObject(string url)
        {
            //ARRANGE
            var handlerMock = GetMockMessageHandler();
            var _subjectUnderTest = new RestClient(handlerMock.Object);
            var postData = new StringContent($"{{ \"label\": \"furniture\"}}", Encoding.UTF8, MediaTypeNames.Application.Json);
            
            // ACT
            var result = await _subjectUnderTest.PostAsync<MockResult>(url,postData);

            // ASSERT
            result.Should().NotBeNull(); // this is fluent assertions here...
            result.Id.Should().Be(1);

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
        class MockResult
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

    }
   
}
