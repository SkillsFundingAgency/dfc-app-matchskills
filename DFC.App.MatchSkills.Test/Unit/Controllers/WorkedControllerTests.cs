using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewComponents.Choice;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class WorkedControllerTests
    {
        private const string Path = "Worked";
        private const string CookieName = ".matchSkills-session";
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private ICookieService _cookieService;

        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());

            _cookieService = Substitute.For<ICookieService>();

        }
        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
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
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
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
        public async Task WhenPostBodyCalledWithYes_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(WorkedBefore.Yes) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.Route}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithNo_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(WorkedBefore.No) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.Worked}");
        }


        [Test]
        public async Task WhenPostBodyCalledWithUndefined_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(WorkedBefore.Undefined) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenWorkedControllerInvoked_ThenModelPropertiesCanBeSet()
        {
            var model = new ChoiceComponentModel
            {
                ButtonText = "test",
                LinkText = "test",
                ParentModel = new WorkedCompositeViewModel()
                {
                    HasError = true
                },
                RadioButtons = new List<RadioButtonModel>
                {
                    {new RadioButtonModel {Text = "test", Order = 1, Name = "test", Value = "test", HintText = "Hint",Checked = false}}
                },
                Text = "test",
                FormAction = "Action",
                ErrorSummaryMessage = "SummaryMessage",
                ErrorMessage = "Error",
                HasError = false
            };
        }

        [Test]
        public void WhenCookieIsSet_CookieIsUpdated()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");
            _cookieService.TryGetPrimaryKey(Arg.Any<HttpRequest>(), Arg.Any<HttpResponse>())
                .ReturnsForAnyArgs("This is My Value");

            var result = controller.Head() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();

        }

        [Test]
        public async Task WhenWorkedControllerReceivesPostWithoutCookie_Then_SetCurrentPageToWorked()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");
            
            _cookieService.TryGetPrimaryKey(Arg.Any<HttpRequest>(), Arg.Any<HttpResponse>())
                .ReturnsForAnyArgs("This is My Value");
            await controller.Body(WorkedBefore.Undefined);
            await _sessionService.Received(1).CreateUserSession(Arg.Any<CreateSessionRequest>(),
                Arg.Any<string>());

        }

    }
}
