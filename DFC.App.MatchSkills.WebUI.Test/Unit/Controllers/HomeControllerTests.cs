using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.MatchSkills.WebUI.Controllers;
using DFC.App.MatchSkills.WebUI.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DFC.App.MatchSkills.WebUI.Test.Unit.Controllers
{
    public class HomeControllerTests
    {
        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new HomeController();
            var result = controller.Head();
            var vm = new HeadViewModel
            {
                PageTitle = "Page Title"
            };
            var pageTitle = vm.PageTitle;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

        }
        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new HomeController();
            var result = controller.Body();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

        }
        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new HomeController();
            var result = controller.Breadcrumb();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new HomeController();
            var result = controller.BodyTop();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
        [Test]
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var controller = new HomeController();
            var result = controller.SidebarRight();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
    }
}
