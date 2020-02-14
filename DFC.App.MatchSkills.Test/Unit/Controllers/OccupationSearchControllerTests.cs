using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Service;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{

    public class OccupationSearchControllerTests
    {
        private IOptions<ServiceTaxonomySettings> _settings;
        private IOptions<CompositeSettings> _compositeSettings;
        private ServiceTaxonomyRepository _serviceTaxonomyRepository;
        private const string Path = "OccupationSearch";
        private ISessionService _sessionService;
        private ICookieService _cookieService;

        [SetUp]
        public void Init()
        {
            _settings = Options.Create(new ServiceTaxonomySettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.SearchOccupationInAltLabels ="true";
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());

            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = MockHelpers.GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());

            _cookieService = new CookieService(new EphemeralDataProtectionProvider());


        }
        
        [Test]
        public void  When_OccupationSearch_Then_ShouldReturnOccupations()
        {
            var sut = new OccupationSearchController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService, _cookieService);
            
            var occupations =   sut.OccupationSearch("renewable");
            
            occupations.Result.Should().NotBeNull();
            occupations.Result.Count().Should().BeGreaterThan(0);
        }

        [Test]
        public void  When_OccupationSearchAuto_Then_ShouldReturnOccupations()
        {
            var sut = new OccupationSearchController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService, _cookieService);
            
            var occupations =   sut.OccupationSearchAuto("Renewable");
            
            occupations.Result.Should().NotBeNull();
        }

        [Test]
        public void  When_GetOccupationIdFromName_Then_ShouldReturnOccupationId()
        {
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"Renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = MockHelpers.GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);
            var sut = new OccupationSearchController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService,_cookieService);
            
            var result =   sut.GetOccupationIdFromName("Renewable energy consultant");

            result.Result.Should().Be("http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197");
        }

        [Test]
        public void When_SearchSkill_Then_RedirectToSelectSkillsView()
        {
            
            var _sessionService = Substitute.For<ISessionService>();
            const string skillsJson ="{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"Renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";           
            var handlerMock = MockHelpers.GetMockMessageHandler(skillsJson);
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);

            var sut = new OccupationSearchController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService,_cookieService);
           sut.ControllerContext = new ControllerContext{
               HttpContext = new DefaultHttpContext()
           };

           sut.ControllerContext=MockHelpers.GetControllerContext();
           _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(MockHelpers.GetUserSession());

            sut.GetSkillsForOccupation("Renewable energy consultant") ;
            
        }


        [Test]
        public void When_HeadCalled_ReturnHtml()
        {
            var sut = new OccupationSearchController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService, _cookieService);
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = sut.Head() as ViewResult;
            
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();

        }

        [Test]
        public async Task WhenBody_Called_ReturnHtml()
        {
            var sut = new OccupationSearchController(_serviceTaxonomyRepository,_settings,_compositeSettings, _sessionService, _cookieService);
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await sut.Body() as ViewResult;

            await _sessionService.Received(1).UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                string.Equals(x.CurrentPage, CompositeViewModel.PageId.OccupationSearch.Value,
                    StringComparison.InvariantCultureIgnoreCase)));

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            result.ViewData.Model.As<OccupationSearchCompositeViewModel>().HasError.Should().BeFalse();
        }
       
    }
}
