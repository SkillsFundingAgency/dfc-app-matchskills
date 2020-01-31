﻿using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class RouteControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IOptions<CompositeSettings> _compositeSettings;

        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _compositeSettings = Options.Create(new CompositeSettings());

        }


        [Test]
        public void WhenPostBodyCalledWithJobs_ReturnHtml()
        {
            var controller = new RouteController(_dataProtectionProvider, _compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(Route.Jobs) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.OccupationSearch}");
        }

        [Test]
        public void WhenPostBodyCalledWithJobsAndSkills_ReturnHtml()
        {
            var controller = new RouteController(_dataProtectionProvider, _compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(Route.JobsAndSkills) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
