using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Service;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class SessionControllerUnitTests
    {
        private const string CookieName = ".matchSkills-session";
        private IDataProtectionProvider _dataProtectionProvider;
        private IDataProtector _dataProtector;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private ICookieService _cookieService;
        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(SessionController));
            _compositeSettings = Options.Create(new CompositeSettings());

            _cookieService = new CookieService(new EphemeralDataProtectionProvider());
        }

        [Test]
        public async Task When_SessionIdProvided_Then_CookieIsAppended()
        {
            // Arrange.
            var controller = new HomeController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            //controller.ControllerContext.HttpContext.Session.Set("foo", null);   //  @ToDo: need to start a session for a unit test

            // Act.
            var result = await controller.Body() as ViewResult;

            // Assert.
            result.Should().NotBeNull();
        }

        [Test]
        public void WhenSessionExpired_ReturnRemoveSession()
        {
            var value = "Value";

            var context = Substitute.For<HttpContext>();
            context.Request.Cookies.TryGetValue(Arg.Any<string>(), out Arg.Any<string>()).Returns(x =>
            {
                x[1] = value;
                return true;
            });

            var controller = new HomeController(_compositeSettings, _sessionService, _cookieService)
            {
                TempData = Substitute.For<ITempDataDictionary>()
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context,
            };


            controller.Head();
        }
    }
}
