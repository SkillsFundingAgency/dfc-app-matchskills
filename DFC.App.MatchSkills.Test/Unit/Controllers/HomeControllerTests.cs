using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class HomeControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;

        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();

        }
        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.Head() as ViewResult;
            var vm = new HeadViewModel
            {
                PageTitle = "Page Title",
                
            };
            var pageTitle = vm.PageTitle;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/Head.cshtml");


        }
        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/Body.cshtml");

        }
        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/BreadCrumb.cshtml");
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/BodyTop.cshtml");
        }
        [Test]
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.SidebarRight() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/SidebarRight.cshtml");
        }
    }
}
