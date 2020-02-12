using System.Linq;
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
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string CookieName = ".matchSkills-session";
        private IDataProtectionProvider _dataProtectionProvider;
        private IDataProtector _dataProtector;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private IOptions<ServiceTaxonomySettings> _settings;
        private ServiceTaxonomyRepository _serviceTaxonomyRepository;

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
            _serviceTaxonomyRepository = new ServiceTaxonomyRepository(restClient);

            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _compositeSettings = Options.Create(new CompositeSettings());
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(SessionController));
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());

        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.Head() as ViewResult;
            var vm = result.ViewData.Model as HeadViewModel;

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new BasketController(_dataProtectionProvider, _compositeSettings, _sessionService);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");
            var requestCookie = new Mock<IRequestCookieCollection>();

            string data = _dataProtector.Protect("This is my value");
            requestCookie.Setup(x =>
                x.TryGetValue(It.IsAny<string>(), out data)).Returns(true);
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new Mock<HttpRequest>();
            var httpResponse = new Mock<HttpResponse>();

            httpResponse.Setup(x => x.Cookies).Returns(new Mock<IResponseCookies>().Object);
            httpRequest.Setup(x => x.Cookies).Returns(requestCookie.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            controller.ControllerContext.HttpContext = httpContext.Object;




            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        /*
         // WIP - refactor the tests which need a session
        

        [Test]
        public void WhenPostBodyCalled_ReturnHtml()
        {
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenSessionIdIsNotNamedCorrectlySet_NoCookieIsSaved()
        {
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
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
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenSessionIdIsSet_CookieIsSaved()
        {
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.Head();
            var headers = controller.Response.Headers;

            headers.Should().ContainKey("set-cookie");
            headers.Values.First().Should().ContainMatch($"{CookieName}*");
        }
    }
}
