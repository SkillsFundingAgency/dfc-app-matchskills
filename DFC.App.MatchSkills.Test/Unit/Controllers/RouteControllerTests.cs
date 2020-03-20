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
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Service;
using NSubstitute.ReturnsExtensions;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class RouteControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private IOptions<DysacSettings> _dysacServiceSetings;
        private IDysacSessionReader _dysacService; 


        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _dysacServiceSetings = Options.Create(new DysacSettings());
            _dysacServiceSetings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/something";
            _dysacServiceSetings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _dysacServiceSetings.Value.DysacUrl="http://dysacurl";
            _dysacService = Substitute.For<IDysacSessionReader>();
            _dysacService.InitiateDysac().ReturnsForAnyArgs(new DysacServiceResponse()
            {
                ResponseCode = DysacReturnCode.Ok
            });
            
            var userSession = new UserSession()
            {
                UserSessionId = "sd",
                PartitionKey = "Key",
                CurrentPage = "string",
                DysacJobCategories = new DysacJobCategory[1],
                LastUpdatedUtc = DateTime.UtcNow,
                Occupations = new HashSet<UsOccupation>(){ new UsOccupation("1","Occupation 1"), new UsOccupation("2","Occupation 1") },
                PreviousPage = "previous",
                Salt = "salt",
                RouteIncludesDysac = true,
                Skills = new HashSet<UsSkill>(){ new UsSkill("1","skill1"), new UsSkill("2","skill2") },
                UserHasWorkedBefore = true
            };
            _sessionService.GetUserSession().ReturnsForAnyArgs(userSession);
        }

        [Test]
        public async Task When_BodyCalledWithoutCookie_Then_ThowError()
        {

            _sessionService.GetUserSession().ReturnsNullForAnyArgs();
            var controller = new RouteController(_compositeSettings, _sessionService ,_dysacService, _dysacServiceSetings);
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
            var controller = new RouteController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.Model.As<RouteCompositeViewModel>().RouteIncludesDysac.Should().Be(true);
        }

        [Test]
        public async Task WhenPostBodyCalledWithJobs_ReturnHtml()
        {
            var controller = new RouteController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(Route.Jobs) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.OccupationSearch}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithJobsAndSkills_ReturnHtml()
        {
            var controller = new RouteController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(Route.JobsAndSkills) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("http://dysacurl");
        }

        [Test]
        public async Task WhenPostBodyCalledWithUndefined_ReturnHtml()
        {
            var controller = new RouteController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body(Route.Undefined) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Route}?errors=true");
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
            var controller = new RouteController(_compositeSettings, _sessionService,_dysacService, _dysacServiceSetings );
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
