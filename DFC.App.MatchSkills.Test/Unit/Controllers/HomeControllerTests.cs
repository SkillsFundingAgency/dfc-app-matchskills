using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class HomeControllerTests
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
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider,_compositeSettings, _sessionService);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var result = controller.Head() as ViewResult;
            var vm = new HeadViewModel
            {
                PageTitle = "Page Title",
                
            };
            var pageTitle = vm.PageTitle;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();

        }
        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider,_compositeSettings, _sessionService);
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
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider,_compositeSettings, _sessionService);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider,_compositeSettings, _sessionService);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        
    }
}
