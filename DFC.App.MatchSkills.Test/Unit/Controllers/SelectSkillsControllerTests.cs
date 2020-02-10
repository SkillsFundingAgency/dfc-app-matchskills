using System;
using System.Linq;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Azure.Cosmos;
using NSubstitute;
using FluentAssertions.Common;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{

    public class SelectSkillsControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IDataProtector _dataProtector;
        private IOptions<ServiceTaxonomySettings> _settings;
        private ServiceTaxonomyRepository _serviceTaxonomyRepository;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private IOptions<SessionSettings> _sessionSettings;
        private IOptions<CosmosSettings> _cosmosSettings;
        private Mock<CosmosClient> _client;
        private ICosmosService _cosmosService;
        
        [SetUp]
        public void Init()
        {
            
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(BaseController));
            _settings = Options.Create(new ServiceTaxonomySettings());
            _compositeSettings = Options.Create(new CompositeSettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.EscoUrl = "http://data.europa.eu/esco";
            _settings.Value.SearchOccupationInAltLabels ="true";
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);

            //Cosmos  Settings
            _cosmosSettings = Options.Create(new CosmosSettings()
            {
                ApiUrl = "https://test-account-not-real.documents.azure.com:443/",
                ApiKey = "VGhpcyBpcyBteSB0ZXN0",
                DatabaseName = "DatabaseName",
                UserSessionsCollection = "UserSessions"
            });
            _client = new Mock<CosmosClient>();
            _cosmosService = Substitute.For<ICosmosService>();

            //Session Settings
            _sessionService = Substitute.For<ISessionService>();
            _sessionSettings = Options.Create(new SessionSettings(){Salt = "ThisIsASalt"});

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
            var vm = new SelectSkillsCompositeViewModel();
            
            // ACTs
            var result = await subjectUnderTest.GetAllSkillsForOccupation<Skill[]>(url,apiKey,"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197") ;
            vm.Skills = result.ToList();
            //var skills = vm.Skills.ToList();
            
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
        public void  When_GetOccupationIdFromName_Then_ShouldReturnOccupationId()
        {
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"Renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);
            var sut = new SelectSkillsController(_dataProtector,_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService);
            
            var result =   sut.GetOccupationIdFromName("Renewable energy consultant");

            result.Result.Should().Be("114e1eff-215e-47df-8e10-45a5b72f8197");
            
        }
        
         [Test]
        public async Task When_AddSkillsForOccupation_Then_ShouldAddSelectedSkills()
        {
            // ARRANGE

            var subFormsCollection = Substitute.For<IFormCollection>();
            var subSessionService = Substitute.For<ISessionService>();

            var sut = new SelectSkillsController(_dataProtector,_serviceTaxonomyRepository,_settings,_compositeSettings, subSessionService);
            
           
            //Arrange
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            
            
            sut.ControllerContext.HttpContext.Response.Cookies.Append(".matchSkills-session", "session5-gn84ygzmm4893m", new CookieOptions
            {
                Secure = true,
                IsEssential = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            

            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent
                        (JsonConvert.SerializeObject(new UserSession()))
                }));
            
            // ACTs
            var result=sut.AddSkills(subFormsCollection);

            // ASSERT
            result.Should().NotBeNull();
            
        }

        #region CUIScaffoldingTests

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new SelectSkillsController(_dataProtector,_serviceTaxonomyRepository,_settings, _compositeSettings, _sessionService);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Head() as ViewResult;
           
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        
        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new SelectSkillsController(_dataProtector,_serviceTaxonomyRepository,_settings, _compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }


       
        #endregion
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
