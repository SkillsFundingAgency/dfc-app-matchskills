using System.Linq;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(SessionController));
            _compositeSettings = Options.Create(new CompositeSettings());
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
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var controller = new BasketController(_dataProtectionProvider,_compositeSettings, _sessionService);
            var result = controller.SidebarRight() as ViewResult;
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
    }
}
