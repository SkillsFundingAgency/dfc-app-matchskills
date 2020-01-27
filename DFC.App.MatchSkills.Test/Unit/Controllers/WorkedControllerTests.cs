using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.ViewComponents.Choice;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class WorkedControllerTests
    {
        private const string Path = "Worked";

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new WorkedController();
            var result = controller.Head() as ViewResult;
            var vm = result.ViewData.Model as HeadViewModel;

            vm.PageTitle.Should().BeEquivalentTo(Path);
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/Head.cshtml");

        }

        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new WorkedController();
            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/Worked/body.cshtml");
        }

        [Test]
        public void WhenPostBodyCalled_ReturnHtml()
        {
            var controller = new WorkedController();
            var result = controller.Body("Yes") as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/Worked/body.cshtml");
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new WorkedController();
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/Breadcrumb.cshtml");
        }

        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new WorkedController();
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/Shared/BodyTopWithOutBanner.cshtml");
        }

        [Test]
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var controller = new WorkedController();
            var result = controller.SidebarRight() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("/Views/home/sidebarright.cshtml");
        }

        [Test]
        public void WhenWorkedControllerInvoked_ThenModelPropertiesCanBeSet()
        {
            var model = new ChoiceComponentModel
            {
                ButtonText = "test",
                LinkText = "test",
                PageId = "test",
                RadioButtons = new List<RadioButtonModel>
                {
                    {new RadioButtonModel {Text = "test", Order = 1, Name = "test"}}
                },
                Text = "test",
                Title = "test"
            };
        }
    }
}
