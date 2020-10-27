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
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        [TestCase("https://dev.api.nationalcareersservice.org.uk", "key")]
        public async Task When_GetSkillsByLabel_Then_ShouldReturnSkillsList(string url, string apiKey)
        {
            // ARRANGE
            const string skillsJson = "{  " +
                                      "\"skills\": [" +
                                      "{    \"skillType\": \"knowledge\",    \"skill\": \"toxicology\",    \"lastModified\": \"2016-12-20T19:32:45Z\",    \"alternativeLabels\": [\"study of toxicity\", \"chemical toxicity\", \"study of adverse effects of chemicals\", \"studies of toxicity\"],    \"uri\": \"http:\\/\\/data.europa.eu\\/esco\\/skill\\/b70ab677-5781-40b5-9198-d98f4a34310f\",    \"matches\": {      \"hiddenLabels\": []," + "      \"skill\": [\"toxicology\"],      \"alternativeLabels\": [\"study of toxicity\", \"chemical toxicity\", \"studies of toxicity\"]    },    \"skillReusability\": \"cross-sectoral\"  }, " +
                                      "{    \"skillType\": \"competency\",    \"skill\": \"perform toxicological studies\",    \"lastModified\": \"2016-12-20T19:37:05Z\",    \"alternativeLabels\": [\"apply toxicological testing methods\", \"perform toxicological tests\", \"perform toxicological study\", \"carry out toxicological studies\"],    \"uri\": \"http:\\/\\/data.europa.eu\\/esco\\/skill\\/000bb1e4-89f0-4b86-be05-05ece3641724\",    \"matches\": {      \"hiddenLabels\": [],      \"skill\": [\"perform toxicological studies\"],      \"alternativeLabels\": [\"apply toxicological testing methods\", \"perform toxicological tests\", \"perform toxicological study\", \"carry out toxicological studies\"]    },    \"skillReusability\": \"cross-sectoral\"  }, " +
                                      "{    \"skillType\": \"knowledge\",    \"skill\": \"food toxicity\",    \"lastModified\": \"2016-12-20T19:05:31Z\",    \"alternativeLabels\": [\"food spoilage\", \"prevention of food poisoning\", \"toxicity of foods\", \"food poisoning\", \"the  toxicity of food\"],    \"uri\": \"http:\\/\\/data.europa.eu\\/esco\\/skill\\/4e081e0a-e25f-4f6e-9c75-e9043ba08aad\",    \"matches\": {      \"hiddenLabels\": [],      \"skill\": [\"food toxicity\"],      \"alternativeLabels\": [\"toxicity of foods\", \"the  toxicity of food\"]    },    \"skillReusability\": \"sector-specific\"  }]}";
            var handlerMock = GetMockMessageHandler(skillsJson, HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);

            // ACTs
            var result = await subjectUnderTest.GetSkillsByLabel<Skill[]>(url, apiKey, "toxic");

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
        [Test]
        public void When_StLabelSkills_CreatedAllPropertiesSet()
        {
            var sut = new StLabelSkills()
            {
                Skills = new StLabelSkill[]
                {
                    new StLabelSkill()
                    {
                        Uri="skilluri",
                        Skill="Skill Name",
                        AlternativeLabels = new [] {"label1","label2"},
                        SkillReusability = "SkillReusability",
                        LastModified =Convert.ToDateTime("1-Oct-2010"),
                        SkillType = "SkillType",
                        RelationshipType = "RelationshipType",
                        Matches = new Matches()
                        {
                            Skill = new string[1]{"test"},
                            AlternativeLabels = new string[1]{"test"},
                            HiddenLabels = new string[1]{"test"},
                        }

                    }
                }
            };

            sut.Skills[0].Uri.Should().Be("skilluri");
            sut.Skills[0].Skill.Should().Be("Skill Name");
            sut.Skills[0].AlternativeLabels[0].Should().Be("label1");
            sut.Skills[0].SkillReusability.Should().Be("SkillReusability");
            var lastModified = sut.Skills[0].LastModified;
            sut.Skills[0].SkillType.Should().Be("SkillType");
            sut.Skills[0].RelationshipType.Should().Be("RelationshipType");
            var matchers = sut.Skills[0].Matches;

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
        public void When_STOccupationSkills_CreatedAllPropertiesSet()
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
                        RelationshipType = RelationshipType.Essential,
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
            sut.Skills[0].RelationshipType.Should().Be(RelationshipType.Essential);

        }

        [Test]
        public void When_GetOccupationsWithMatchingSkillsRequestCreated_Then_SkillsListShouldBeInitialised()
        {
            // Arrange


            // Act
            var x = new GetOccupationsWithMatchingSkillsRequest();

            // Assert
            x.SkillList.Should().NotBeNull();
            x.SkillList.Should().HaveCount(0);
        }

        [Test]
        public void When_GetOccupationsWithMatchingSkillsRequestInitialised_Then_PropertiesShouldHaveValues()
        {
            // Arrange


            // Act
            var x = new GetOccupationsWithMatchingSkillsRequest()
            {
                MinimumMatchingSkills = 7,
            };

            // Assert
            x.MinimumMatchingSkills.Should().Be(7);
        }

        [Test]
        public void When_GetOccupationsWithMatchingSkillsResponseCreated_Then_SkillsListShouldBeInitialised()
        {
            // Arrange


            // Act
            var x = new GetOccupationsWithMatchingSkillsResponse();

            // Assert
            x.MatchingOccupations.Should().NotBeNull();
            x.MatchingOccupations.Should().HaveCount(0);
        }

        [Test]
        public void When_MatchedOccupationInitialised_Then_PropertiesShouldHaveValues()
        {
            // Arrange


            // Act
            var x = new GetOccupationsWithMatchingSkillsResponse.MatchedOccupation()
            {
                JobProfileTitle = "testValue",
                JobProfileUri = "anotherTestValue",
                MatchingEssentialSkills = 1,
                MatchingOptionalSkills = 2,
                LastModified = DateTime.UtcNow,
                Uri = "testValue",
                TotalOccupationEssentialSkills = 3,
                TotalOccupationOptionalSkills = 4,
            };

            // Assert
            x.JobProfileTitle.Should().Be("testValue");
            x.JobProfileUri.Should().Be("anotherTestValue");
            x.Uri.Should().Be("testValue");
            x.MatchingEssentialSkills.Should().Be(1);
            x.MatchingOptionalSkills.Should().Be(2);
            x.TotalOccupationEssentialSkills.Should().Be(3);
            x.TotalOccupationOptionalSkills.Should().Be(4);
            x.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 1, 0));
        }


        [Test]
        public void When_OccupationMatchInitialised_Then_PropertiesShouldHaveValues()
        {
            // Arrange


            // Act
            var x = new OccupationMatch()
            {
                JobProfileTitle = "testValue",
                JobProfileUri = "anotherTestValue",
                MatchingEssentialSkills = 1,
                MatchingOptionalSkills = 2,
                LastModified = DateTime.UtcNow,
                Uri = "testValue",
                TotalOccupationEssentialSkills = 3,
                TotalOccupationOptionalSkills = 4,
            };

            // Assert
            x.JobProfileTitle.Should().Be("testValue");
            x.JobProfileUri.Should().Be("anotherTestValue");
            x.Uri.Should().Be("testValue");
            x.MatchingEssentialSkills.Should().Be(1);
            x.MatchingOptionalSkills.Should().Be(2);
            x.TotalOccupationEssentialSkills.Should().Be(3);
            x.TotalOccupationOptionalSkills.Should().Be(4);
            x.LastModified.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 1, 0));
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk", "key")]
        public async Task When_SkillMatchInitialised_Then_SkillResultShouldBePopulated(string url, string apiKey)
        {
            // ARRANGE
            var stresponse = new GetOccupationsWithMatchingSkillsResponse();
            stresponse.MatchingOccupations.Add( 
                new GetOccupationsWithMatchingSkillsResponse.MatchedOccupation()
                {
                    JobProfileTitle = "Baggage Handler",
                    JobProfileUri = "http://jobprofile",
                    LastModified = DateTime.UtcNow,
                    MatchingEssentialSkills = 5,
                    MatchingOptionalSkills = 3,
                    TotalOccupationEssentialSkills = 10,
                    TotalOccupationOptionalSkills = 4,
                });
            string matchedOccupationJson = JsonConvert.SerializeObject(stresponse);
            var handlerMock = GetMockMessageHandler(matchedOccupationJson);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            var skillIds = new string[]
            {
                "skill1",
                "skill2",
                "skill3"
            };
            var minimumMatchingSkills = 1;

            // ACTs
            var result = await subjectUnderTest.FindOccupationsForSkills(url, apiKey, skillIds, minimumMatchingSkills);

            // ASSERT
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].JobProfileTitle.Should().Be("Baggage Handler");

            /*
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                ),
                ItExpr.IsAny<CancellationToken>()
            );
            */
        }
        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
        public async Task WhenGetSkillsGapForOccupationAndGivenSkills_WithNullParams_Then_ShouldReturnNoContent(string url,string apiKey)
        {
            // ARRANGE
            var handlerMock = GetMockMessageHandler("",HttpStatusCode.NoContent);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.GetSkillsGapForOccupationAndGivenSkills<SkillsGapAnalysis>(url,apiKey,null, null) ;
            
            // ASSERT
            result.Should().BeNull();

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post 
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        
        }

        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
        public async Task WhenGetSkillsGapForOccupationAndGivenSkills_Then_ShouldReturnSKillsAnalysis(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson = "{  \"occupation\": \"grants management officer\",  \"missingSkills\": [{    \"relationshipType\": \"essential\",    " +
                                      "\"skill\": \"keep grant applicant informed\",    \"lastModified\": \"2016-12-20T20:16:59Z\",    \"alternativeLabels\": [\"keep grant applicant notified\", \"keep scholarship applicant notified\", \"keep grant applicant advised\", \"keep grant applicant briefed\", \"keep scholarship applicant advised\", \"keep scholarship applicant briefed\"],    \"type\": \"competency\",    \"uri\": \"http://data.europa.eu/esco/skill/c0f8207c-2377-4fe2-abc4-b173cc4c31c8\",    \"skillReusability\": \"occupation-specific\"  }, {    \"relationshipType\": \"essential\",    " +
                                      "\"skill\": \"grant concessions\",    \"lastModified\": \"2016-12-20T20:13:59Z\",    \"alternativeLabels\": [\"issue concessions\", \"develop concessionary policies\", \"allow concessionary policies\", \"agree concessionary policies\", \"agree concessions\", \"allow concessions\", \"enable concessions\", \"enable concessionary policies\"],    \"type\": \"competency\",    \"uri\": \"http://data.europa.eu/esco/skill/1ccf5bda-d904-4d00-9c71-9b8f0196e9f9\",    \"skillReusability\": \"sector-specific\"  }, {    \"relationshipType\": \"essential\",    " +
                                      "\"skill\": \"report on grants\",    \"lastModified\": \"2016-12-20T20:16:12Z\",    \"alternativeLabels\": [\"summarise grants\", \"administer allocations\", \"report donor and recipient\", \"detail grants\", \"administer grants summarise grants\", \"summarise allocations\", \"detail allocations\"],    \"type\": \"competency\",    \"uri\": \"http://data.europa.eu/esco/skill/0b6cc8e4-b34d-4631-9061-3ba839ecc640\",    \"skillReusability\": \"occupation-specific\"  }],  " +
                                      "\"jobProfileTitle\": null,  \"matchingSkills\": [],  \"lastModified\": \"2017-02-02T10:53:11Z\",  \"alternativeLabels\": [\"proposals manager\", \"grants officer\"],  \"jobProfileUri\": null,  \"uri\": \"http://data.europa.eu/esco/occupation/89330b57-6a30-40e1-b623-50f47a4b0c34\"}";
            var handlerMock = GetMockMessageHandler(skillsJson, HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            
            // ACTs
            var result = await subjectUnderTest.GetSkillsGapForOccupationAndGivenSkills<SkillsGapAnalysis>(url,apiKey,null, null) ;
            
            // ASSERT
            result.Should().NotBeNull();
            result.CareerTitle.Should().Be("grants management officer");
            var description = result.CareerDescription;
            result.MissingSkills.Length.Should().Be(3);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post 
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        
        }
        [Test]
        public void WhenSkillsGapAnalysisCreated_Assign_RetrieveValues()
        {
            var skillsGapAnalysis = new SkillsGapAnalysis()
            {
                MissingSkills = new StOccupationSkills.StOsSkill[1]{ new StOccupationSkills.StOsSkill()},
                MatchingSkills = new StOccupationSkills.StOsSkill[1]{ new StOccupationSkills.StOsSkill()},
                AlternativeLabels = new string[1]{"Label"},
                JobProfileTitle = "JobProfileTitle",
                JobProfileUri = new Uri("https://dev.api.nationalcareersservice.org.uk/test/"),
                LastModified = DateTimeOffset.UtcNow,
                Occupation = "Occupation",
                Uri = new Uri("https://dev.api.nationalcareersservice.org.uk/test/")
            };
            var missingSkills = skillsGapAnalysis.MissingSkills;
            var matchingSkills = skillsGapAnalysis.MatchingSkills;
            var alternativeLabels = skillsGapAnalysis.AlternativeLabels;
            var jobProfileTitle = skillsGapAnalysis.JobProfileTitle;
            var jobProfileUri = skillsGapAnalysis.JobProfileUri;
            var lastModified = skillsGapAnalysis.LastModified;
            var occupation = skillsGapAnalysis.Occupation;
            var uri = skillsGapAnalysis.Uri;
        }

        [Test]
        public void WhenSkillsGapRequestMade_ReturnValues()
        {
            var request = new SkillsGapRequest()
            {
                Occupation = "Occupation",
                SkillList = new string[1]
                {
                    "Skills"
                }
            };
            var occupation = request.Occupation;
            var skills = request.SkillList;

            occupation.Should().Be("Occupation");
            skills[0].Should().Be("Skills");
        }
        class MockResult
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

    }
   
}
