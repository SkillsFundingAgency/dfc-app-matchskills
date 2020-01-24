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
            var result = controller.Head();
            var vm = new HeadViewModel
            {
                PageTitle = "Page Title",
                DefaultCssLink = "Link"
                
            };
            var pageTitle = vm.PageTitle;
            var css = vm.DefaultCssLink;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

        }
        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.Body();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

        }
        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.Breadcrumb();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.BodyTop();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
        [Test]
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var controller = new HomeController(_dataProtectionProvider);
            var result = controller.SidebarRight();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
    }
}
