using System.Collections.Generic;
using DFC.App.MatchSkills.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class ReloadControllerTests
    {
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
            _dysacServiceSetings = Options.Create(new DysacSettings
            {
                DysacReturnUrl = "DysacRoute"
            });
            _dysacServiceSetings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/something";
            _dysacServiceSetings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _dysacServiceSetings.Value.DysacUrl = "http://dysacurl";
            _dysacService = Substitute.For<IDysacSessionReader>();
            
        }

        [Test]
        public async Task WhenBodyCalledWithEmptyString_RedirectToHome()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/");
        }

        [Test]
        public async Task WhenBodyCalledAndSessionIdIsValid_RedirectToUsersLastPage()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession()
                {CurrentPage = CompositeViewModel.PageId.Matches.Value});
            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Matches}");
        }

        [Test]
        public async Task WhenBodyCalledAndSessionIdIsInvalid_RedirectToHome()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsNullForAnyArgs();
            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/home?errors=true");
        }

        [Test]
        public async Task WhenPostCalledAndSessionIdIsInvalid_RedirectToHomeWithErrors()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsNullForAnyArgs();
            var result = await controller.Body("123") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/home?errors=true");
        }

        [Test]
        public async Task WhenPostCalledAndSessionIdIsValid_RedirectToUsersLastPage()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession()
                { CurrentPage = CompositeViewModel.PageId.Matches.Value });
            var result = await controller.Body("123") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Matches}");
        }

        [Test]
        public async Task WhenPostCalledAndSessionIdIsDysacSessionOnly_RedirectToDysac()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession()
            {
                CurrentPage = CompositeViewModel.PageId.Matches.Value,
                UserHasWorkedBefore = false

            });
            var result = await controller.Body("123") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/DysacRoute");
        }

        [Test]
        public async Task WhenBodyCalledAndSessionIdIsDysacSessionOnly_RedirectToDysac()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession()
            {
                CurrentPage = CompositeViewModel.PageId.Matches.Value,
                UserHasWorkedBefore = false

            });
            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/DysacRoute");
        }

        [Test]
        public async Task WhenPostCalledAndSessionIdIsDysacAndSkillsSessionAndLastPageIsRouteWithOption2Selected_RedirectToDysac()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession()
            {
                CurrentPage = CompositeViewModel.PageId.Route.Value,
                UserHasWorkedBefore = true,
                RouteIncludesDysac = true

            });
            var result = await controller.Body("123") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/DysacRoute");
        }

        [Test]
        public async Task WhenBodyCalledAndSessionIdIsDysacAndSkillsSessionAndLastPageIsRouteWithOption2Selected_RedirectToDysac()
        {
            var controller = new ReloadController(_compositeSettings, _sessionService, _dysacServiceSetings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"sessionId","123" }
            });
            _sessionService.Reload(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession()
            {
                CurrentPage = CompositeViewModel.PageId.Route.Value,
                UserHasWorkedBefore = true,
                RouteIncludesDysac = true

            });
            var result = await controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/DysacRoute");
        }

    }
}
