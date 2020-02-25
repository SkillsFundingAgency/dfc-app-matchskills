using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class RemovedControllerTests
    {
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
        public async Task When_ConfirmRemoveCalled_ReturnView()
        {
            var controller = new RemovedController(_compositeSettings, _sessionService, _cookieService);
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
        public async Task When_RemovedCalled_TrackTrackPageInUserSession()
        {
            var controller = new RemovedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.Body() as ViewResult;

            await _sessionService.Received()
                .UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                    x.CurrentPage == CompositeViewModel.PageId.Removed.Value));
        }

        [Test]
        public async Task When_RemovedCalledAndUserStillHasSkillsInTheBasket_ThenReturnViewWithHasMoreSkills()
        {
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession
            {
                SkillsToRemove = new HashSet<UsSkill>
                {
                    new UsSkill("1", "test", DateTime.Now)
                },
                Skills = new HashSet<UsSkill>
                {
                    new UsSkill("1", "test", DateTime.Now)
                }
            });


            var controller = new RemovedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.Body() as ViewResult;
            result.Model.As<RemovedCompositeViewModel>().HasRemainingSkills.Should().BeTrue();
            result.Model.As<RemovedCompositeViewModel>().Skills.Count.Should().Be(1);
        }


        [Test]
        public async Task When_RemovedPostedTo_ReturnRedirect()
        {
            var controller = new RemovedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var formDictionary = new Dictionary<string, StringValues>
            {
                {"test--test1", "test"}
            };

            var formCollection = new FormCollection(formDictionary);

            var result = await controller.Body(formCollection) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.Removed}");
        }


        [Test]
        public async Task When_RemovedPostedTo_Then_SkillsRemoved()
        {
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession
            {
                SkillsToRemove = new HashSet<UsSkill>
                {
                    new UsSkill("1", "test", DateTime.Now)
                },
                Skills = new HashSet<UsSkill>
                {
                    new UsSkill("1", "test", DateTime.Now)
                }
            });


            var controller = new RemovedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var formDictionary = new Dictionary<string, StringValues>
            {
                {"1--test1", "test"}
            };

            var formCollection = new FormCollection(formDictionary);

            await controller.Body(formCollection);

            await _sessionService.Received()
                .UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                    x.CurrentPage == CompositeViewModel.PageId.Removed.Value && x.SkillsToRemove.Count == 1 && x.Skills.Count == 0));

        }

        [Test]
        public async Task When_RemovedPostedToAndFormIsEmpty_Then_RedirectToSkillsBasket()
        {
            var controller = new RemovedController(_compositeSettings, _sessionService, _cookieService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var formDictionary = new Dictionary<string, StringValues>
            {
            };

            var formCollection = new FormCollection(formDictionary);

            await controller.Body(formCollection);

            var result = await controller.Body(formCollection) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.RemoveSkills}?errors=true");

        }

    }
}
