﻿using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
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
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class EnterSkillsControllerTests
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
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new EnterSkillsController(_compositeSettings, _sessionService );
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
        public void WhenPostBodyWithBlankInputCalled_ReturnHtml()
        {
            var controller = new EnterSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body("") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.EnterSkills}?errors=true");
        }
        [Test]
        public void WhenPostBodyWithValidInputCalled_ReturnHtml()
        {
            var controller = new EnterSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body("Car") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be("~/relatedSkills?searchTerm=Car");
        }
        [Test]
        public void WhenEnterSkillsControllerInvoked_ThenModelPropertiesCanBeSetAndRetrieved()
        {
            var model = new EnterSkillsCompositeViewModel()
            {
                HasError = true,
                CompositeSettings = _compositeSettings.Value,
                Skills = { new Skill("id", "name")}
            };
            var hasError = model.HasError;
            var settings = model.CompositeSettings;
            var skills = model.Skills;
        }

    }
}
