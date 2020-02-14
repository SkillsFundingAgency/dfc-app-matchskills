using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
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
using System;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Service;
using NSubstitute.ReturnsExtensions;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class RouteControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private ICookieService _cookieService;


        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());
            _cookieService = new CookieService(new EphemeralDataProtectionProvider());
        }

        [Test]
        public async Task When_BodyCalledWithoutCookie_Then_ThowError()
        {

            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsNullForAnyArgs();
            var controller = new RouteController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };


            Func<Task> act = async () => { await controller.Body(); };

            await act.Should().ThrowAsync<Exception>();
        }


        [Test]
        public async Task WhenBodyCalled_ThenSessionIsLoaded()
        {
            var controller = new RouteController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.Model.As<RouteCompositeViewModel>().RouteIncludesDysac.Should().BeNull();
        }

        [Test]
        public async Task WhenPostBodyCalledWithJobs_ReturnHtml()
        {
            var controller = new RouteController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(Route.Jobs) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.OccupationSearch}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithJobsAndSkills_ReturnHtml()
        {
            var controller = new RouteController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(Route.JobsAndSkills) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.Route}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithUndefined_ReturnHtml()
        {
            var controller = new RouteController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(Route.Undefined) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenRouteControllerInvoked_ThenModelPropertiesCanBeSet()
        {
            var model = new RouteCompositeViewModel()
            {
                HasError = true
            };
        }

        [Test]
        public async Task WhenRouteControllerReceivesPost_Then_SetCurrentPageToRoute()
        {
            var controller = new RouteController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            await controller.Body(Route.Undefined);
            await _sessionService.Received(1).UpdateUserSessionAsync(Arg.Is<UserSession>(x => 
                string.Equals(x.CurrentPage, CompositeViewModel.PageId.Route.Value, 
                    StringComparison.InvariantCultureIgnoreCase)));

        }
    }

}
