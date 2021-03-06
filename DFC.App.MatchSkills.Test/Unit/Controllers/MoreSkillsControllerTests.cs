﻿using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    class MoreSkillsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
         

        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
             
            _sessionService.GetUserSession().ReturnsForAnyArgs(new UserSession());
        }

        [Test]
        public async Task WhenBodyCalledWithJobs_ReturnHtml()
        {
            var controller = new MoreSkillsController(_compositeSettings, _sessionService );
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
        public async Task WhenPostBodyCalledWithJobs_ReturnHtml()
        {
            var controller = new MoreSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(MoreSkills.Job) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.MoreJobs}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithJobsAndSkills_ReturnHtml()
        {
            var controller = new MoreSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(MoreSkills.Skill) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.EnterSkills}");
        }

        [Test]
        public async Task WhenPostBodyCalledWithUndefined_ReturnHtml()
        {
            var controller = new MoreSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(MoreSkills.Undefined) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.MoreSkills}?errors=true");
        }

        [Test]
        public void WhenMoreSkillsControllerInvoked_ThenModelPropertiesCanBeSet()
        {
            var model = new MoreSkillsCompositeViewModel()
            {
                HasError = true
            };
        }


        [Test]
        public async Task WhenMoreSkillsVisited_Then_SetCurrentPageToMoreSkills()
        {
            var controller = new MoreSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            await controller.Body();
            await _sessionService.Received(1).UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                string.Equals(x.CurrentPage, CompositeViewModel.PageId.MoreSkills.Value,
                    StringComparison.InvariantCultureIgnoreCase)));

        }
    }
}
