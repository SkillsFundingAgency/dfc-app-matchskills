using System.Linq;
using System.Threading.Tasks;
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
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string CookieName = ".matchSkills-session";
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private IOptions<ServiceTaxonomySettings> _settings;
        private ICookieService _cookieService;

        [SetUp]
        public void Init()
        {

            _settings = Options.Create(new ServiceTaxonomySettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.EscoUrl = "http://data.europa.eu/esco";
            _settings.Value.SearchOccupationInAltLabels = "true";
            var handlerMock = MockHelpers.GetMockMessageHandler();
            var restClient = new RestClient(handlerMock.Object);
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());
            _cookieService = Substitute.For<ICookieService>();
            _cookieService.TryGetPrimaryKey(Arg.Any<HttpRequest>(), Arg.Any<HttpResponse>())
                .ReturnsForAnyArgs("This is My Value");
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new BasketController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.Head() as ViewResult;
            var vm = result.ViewData.Model as HeadViewModel;

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new BasketController(_compositeSettings, _sessionService, _cookieService);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");


            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        /*
         // WIP - refactor the tests which need a session
        

        [Test]
        public async Task WhenPostBodyCalled_ReturnHtml()
        {
            var controller = new BasketController(_compositeSettings, _sessionService, _CookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenSessionIdIsNotNamedCorrectlySet_NoCookieIsSaved()
        {
            var controller = new BasketController(_compositeSettings, _sessionService, _CookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.HttpContext.Request.QueryString = QueryString.Create("wrongId", "Abc123");
            controller.Body();
            var headers = controller.Response.Headers;

            headers.Should().NotContainKey("set-cookie");
            headers.Values.Should().NotContain($"{CookieName}*");
        }
        */

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new BasketController(_compositeSettings, _sessionService, _cookieService);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new BasketController(_compositeSettings, _sessionService, _cookieService);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
