using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class ErrorControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;


        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new ErrorController(_compositeSettings, _sessionService);
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
        public async Task When_SessionEmpty_Then_ViewModelHasNoRTACode()
        {
            var controller = new ErrorController(_compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _sessionService.GetUserSession().ReturnsForAnyArgs(MockHelpers.GetUserSession(false));

            await controller.Body();

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            ((ErrorCompositeViewModel)result.ViewData.Model).RTACode.Should().Be(string.Empty);
        }
    }
}
