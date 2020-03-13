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
using AspNetCore;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using NSubstitute.ReturnsExtensions;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class WorkedControllerTests
    {
        private const string Path = "Worked";
        private const string CookieName = ".matchSkills-session";
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private IOptions<DysacSettings> _dysacServiceSetings;
        private IDysacSessionReader _dysacService;
         

        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession().ReturnsForAnyArgs(new UserSession());

             

        }
        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
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
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
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
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(WorkedBefore.Yes) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Route}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithNo_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(WorkedBefore.No) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Worked}");
        }


        [Test]
        public async Task WhenPostBodyCalledWithUndefined_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(WorkedBefore.Undefined) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Worked}?errors=true");
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
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
        public async Task When_VisitingTheWorkedPageWithACookie_Then_CookieIsUpdatedAndCurrentPageIsSetToWorked()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".dfc-session", "Abc123");

            await controller.Body();

            await _sessionService.Received().CreateUserSession(Arg.Is<CreateSessionRequest>(x=> x.CurrentPage == CompositeViewModel.PageId.Worked.Value));
        }


        [Test]
        public async Task When_VisitingTheWorkedPageWithoutACookie_Then_CreateCookie()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");

            await controller.Body();
            await _sessionService.Received(1).CreateUserSession(Arg.Any<CreateSessionRequest>());
        }

        [Test]
        public async Task When_PostingBackToTheWorkedPage_Then_UpdateSessionWithWorkedPageChoice()
        {
            var controller = new WorkedController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");

            await controller.Body(WorkedBefore.Yes);
            await _sessionService.Received().UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                x.UserHasWorkedBefore == true));

        }

    }
}
