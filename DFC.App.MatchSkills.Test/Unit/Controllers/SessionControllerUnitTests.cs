using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(SessionController));
            _compositeSettings = Options.Create(new CompositeSettings());
        }

        [Test]
        public void When_SessionIdProvided_Then_CookieIsAppended()
        {
            // Arrange.
            var controller = new HomeController(_dataProtectionProvider,_compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            //controller.ControllerContext.HttpContext.Session.Set("foo", null);   //  @ToDo: need to start a session for a unit test

            // Act.
            var result = controller.Body() as ViewResult;

            // Assert.
            result.Should().NotBeNull();
        }
    }
}
