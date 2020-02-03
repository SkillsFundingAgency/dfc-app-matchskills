using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewComponents.Choice;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class WorkedControllerTests
    {
        private const string Path = "Worked";
        private const string CookieName = ".matchSkills-session";
        private IDataProtectionProvider _dataProtectionProvider;
        private IOptions<CompositeSettings> _compositeSettings;
        private IDataProtector _dataProtector;

        [SetUp]
        public void Init()
        {
            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _compositeSettings = Options.Create(new CompositeSettings());
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(SessionController));

        }
        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider,_compositeSettings);
            var result = controller.Head() as ViewResult;
            var vm = result.ViewData.Model as HeadViewModel;

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();

        }

        [Test]
        public void WhenBodyCalled_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider, _compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenPostBodyCalledWithYes_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider, _compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(WorkedBefore.Yes) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.Route}");
        }

        [Test]
        public void WhenPostBodyCalledWithNo_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider, _compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Body(WorkedBefore.No) as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"/{CompositeViewModel.PageId.Worked}");
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider, _compositeSettings);
            var result = controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyTopCalled_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider, _compositeSettings);
            var result = controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenSidebarRightCalled_ReturnHtml()
        {
            var controller = new WorkedController(_dataProtectionProvider, _compositeSettings);
            var result = controller.SidebarRight() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenWorkedControllerInvoked_ThenModelPropertiesCanBeSet()
        {
            var model = new ChoiceComponentModel
            {
                ButtonText = "test",
                LinkText = "test",
                ParentModel = new WorkedCompositeViewModel(),
                RadioButtons = new List<RadioButtonModel>
                {
                    {new RadioButtonModel {Text = "test", Order = 1, Name = "test", Value = "test", HintText = "Hint"}}
                },
                Text = "test",
                FormAction = "Action"
            };
        }
        [Test]
        public void WhenSessionIdIsSet_CookieIsSaved()
        {
            var sessionValue = "Abc123";
            var controller = new WorkedController(_dataProtectionProvider,_compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.HttpContext.Request.QueryString = QueryString.Create("sessionId", sessionValue);
            controller.Body();
            var headers = controller.Response.Headers;

            headers.Should().ContainKey("set-cookie");
            headers.Values.First().Should().ContainMatch($"{CookieName}*");
        }

        [Test]
        public void WhenSessionIdIsNotNamedCorrectlySet_NoCookieIsSaved()
        {
            var controller = new WorkedController(_dataProtectionProvider,_compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.HttpContext.Request.QueryString = QueryString.Create("wrongId", "Abc123");
            controller.Body();
            var headers = controller.Response.Headers;

            headers.Should().NotContainKey("set-cookie");
            headers.Values.Should().NotContain($"{CookieName}*");
        }


        [Test]
        public void WhenCookieIsSet_CookieIsUpdated()
        {
            var controller = new WorkedController(_dataProtectionProvider,_compositeSettings);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");
            var requestCookie = new Mock<IRequestCookieCollection>();

            string data = _dataProtector.Protect("This is my value");
            requestCookie.Setup(x => 
                x.TryGetValue(It.IsAny<string>(), out data)).Returns(true);
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new Mock<HttpRequest>();
            var httpResponse = new Mock<HttpResponse>();

            httpResponse.Setup(x => x.Cookies).Returns(new Mock<IResponseCookies>().Object);
            httpRequest.Setup(x => x.Cookies).Returns(requestCookie.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            controller.ControllerContext.HttpContext = httpContext.Object;

            var result = controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();


        }

    }
}
