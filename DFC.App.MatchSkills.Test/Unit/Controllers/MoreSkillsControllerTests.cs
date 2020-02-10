using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.MatchSkills.Application.Session.Interfaces;
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

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    class MoreSkillsControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
  

        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _compositeSettings = Options.Create(new CompositeSettings());
        }


        [Test]
        public void WhenPostBodyCalledWithJobs_ReturnHtml()
        {
            var controller = new MoreSkillsController(_dataProtectionProvider, _compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(MoreSkills.Job) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.OccupationSearch}");
        }

        [Test]
        public void WhenPostBodyCalledWithJobsAndSkills_ReturnHtml()
        {
            var controller = new MoreSkillsController(_dataProtectionProvider, _compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(MoreSkills.Skill) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.MoreSkills}");
        }

        [Test]
        public void WhenPostBodyCalledWithUndefined_ReturnHtml()
        {
            var controller = new MoreSkillsController(_dataProtectionProvider, _compositeSettings, _sessionService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(MoreSkills.Undefined) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenMoreSkillsControllerInvoked_ThenModelPropertiesCanBeSet()
        {
            var model = new MoreSkillsCompositeViewModel()
            {
                HasError = true
            };
        }
    }
}
