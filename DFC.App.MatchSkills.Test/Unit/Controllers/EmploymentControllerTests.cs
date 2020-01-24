using System.Collections.Generic;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class EmploymentControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private EmploymentChoiceController _controller;
        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _controller = new EmploymentChoiceController(_dataProtectionProvider);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { }
            };
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var result = _controller.Head();
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
            var result = _controller.Body();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

        }
        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var result = _controller.Breadcrumb();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var result = _controller.BodyTop();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }
        [Test]
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var result = _controller.SidebarRight();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }

    }
}
