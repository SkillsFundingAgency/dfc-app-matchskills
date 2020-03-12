using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Service;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
        private ICookieService _cookieService;
        
        [SetUp]
        public void Init()
        {
            
            _settings = Options.Create(new ServiceTaxonomySettings());
            _compositeSettings = Options.Create(new CompositeSettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.EscoUrl = "http://data.europa.eu/esco";
            _settings.Value.SearchOccupationInAltLabels ="true";
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = MockHelpers.GetMockMessageHandler(skillsJson);
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
            _cookieService = new CookieService(new EphemeralDataProtectionProvider());

        }
        
        [TestCase("https://dev.api.nationalcareersservice.org.uk","key")]
        public async Task When_GetAllSkillsForOccupation_Then_ShouldReturnSkillsList(string url,string apiKey)
        {
            // ARRANGE
            const string skillsJson ="{skills:[" +
                                     "{\"type\": \"competency\",\"relationshipType\": \"essential\",\"skill\": \"collect biological data\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}," +
                                     "{\"type\": \"competency\",\"relationshipType\": \"essential\",\"skill\": \"collect biological info\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}," +
                                     "{\"type\": \"competency\",\"relationshipType\": \"optional\",\"skill\": \"collect samples\",\"alternativeLabels\": [\"biological data collection\", \"analysing biological records\"],\"uri\": \"aaa\"}]}";
            var handlerMock = MockHelpers.GetMockMessageHandler(skillsJson,HttpStatusCode.OK);
            var restClient = new RestClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(restClient);
            var vm = new SelectSkillsCompositeViewModel();
            
            // ACTs
            var result = await subjectUnderTest.GetAllSkillsForOccupation<Skill[]>(url,apiKey,"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197") ;
            vm.Skills = result.ToList();
           
            
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
        public async Task When_Body_Then_LoadBodyAndGetSessionData()
        {
            var controller = new SelectSkillsController(_serviceTaxonomyRepository, _settings, _compositeSettings, _sessionService, _cookieService)
            {
                ControllerContext = new ControllerContext
                {
                HttpContext = new DefaultHttpContext()
            }
        }; 
            
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(MockHelpers.GetUserSession(true));
            
            await controller.Body();

            await _sessionService.Received().UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                x.CurrentPage == CompositeViewModel.PageId.SelectSkills.Value));
        }

        [Test]
        public async Task When_Toggle_Then_ViewModelAllSkillsSelectedToggled()
        {
            var controller = new SelectSkillsController(_serviceTaxonomyRepository, _settings, _compositeSettings, _sessionService, _cookieService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            }; 
            
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(MockHelpers.GetUserSession(true));

    
            var result = await controller.SkillSelectToggle(false) as ViewResult;
            var vm = result.ViewData.Model as SelectSkillsCompositeViewModel;
            vm.AllSkillsSelected.Should().BeTrue();

        }

        #region CUIScaffoldingTests

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new SelectSkillsController(_serviceTaxonomyRepository,_settings, _compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Head() as ViewResult;
           
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

       
        #endregion
       

    }

    public class TestAddSkills
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private ServiceTaxonomyRepository _serviceTaxonomyRepository;
        private IOptions<ServiceTaxonomySettings> _settings;

        private ICookieService _cookieService;

        [SetUp]
        public void Init()
        {
            
            _settings = Options.Create(new ServiceTaxonomySettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.EscoUrl = "http://data.europa.eu/esco";
            _settings.Value.SearchOccupationInAltLabels ="true";
            var handlerMock = MockHelpers.GetMockMessageHandler();
            var restClient = new RestClient(handlerMock.Object); 
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());
            _cookieService = new CookieService(new EphemeralDataProtectionProvider());

        }
         [Test]
        public async Task When_AddSkillsForOccupation_Then_ShouldAddSelectedSkills()

        {
            
            var controller = new SelectSkillsController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService, _cookieService);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(CookieService.CookieName, "Abc123");
            controller.ControllerContext.HttpContext = MockHelpers.SetupControllerHttpContext().Object;
            
            var dic = new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            dic.Add("somekey--somevalue", "key1");
            dic.Add("somekey1--somevalue1", "key2");
            var collection = new Microsoft.AspNetCore.Http.FormCollection(dic);
           

            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(MockHelpers.GetUserSession(true));
            _sessionService.UpdateUserSessionAsync(Arg.Any<UserSession>()).ReturnsNullForAnyArgs();

            var result = await controller.Body(collection) as RedirectResult;
            
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.SkillsBasket}");
        }
       
        [Test]
        public async Task When_AddSkillsWithNoSkillsPassed_Then_Error()

        {
            
            var controller = new SelectSkillsController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService, _cookieService);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(CookieService.CookieName, "Abc123");
            controller.ControllerContext.HttpContext = MockHelpers.SetupControllerHttpContext().Object;
            
            var dic = new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
          
            var collection = new Microsoft.AspNetCore.Http.FormCollection(dic);
           

            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(MockHelpers.GetUserSession(true));
            _sessionService.UpdateUserSessionAsync(Arg.Any<UserSession>()).ReturnsNullForAnyArgs();

            var result = await controller.Body(collection) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.SelectSkills}?errors=true");

        }
    }

}
