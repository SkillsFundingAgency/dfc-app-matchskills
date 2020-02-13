using System;
using System.Collections.Generic;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class MoreJobsControllerTests
    {
        private IDataProtectionProvider _dataProtectionProvider;
        private IDataProtector _dataProtector;
        private IOptions<ServiceTaxonomySettings> _settings;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;

        [SetUp]
        public void Init()
        {

            _settings = Options.Create(new ServiceTaxonomySettings());
            _settings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/servicetaxonomy";
            _settings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
            _settings.Value.EscoUrl = "http://data.europa.eu/esco";
            _settings.Value.SearchOccupationInAltLabels = "true";
            var handlerMock = MockHelpers.GetMockMessageHandler();
            var restClient = new RestClient(handlerMock.Object);

            _dataProtectionProvider = new EphemeralDataProtectionProvider();
            _compositeSettings = Options.Create(new CompositeSettings());
            _dataProtector = _dataProtectionProvider.CreateProtector(nameof(SessionController));
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());

        }

        [Test]
        public async Task WhenBody_Called_ReturnHtml()
        {
            var controller =
                new MoreJobsController(_dataProtectionProvider, _settings, _compositeSettings, _sessionService);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");
            var requestCookie = new Mock<IRequestCookieCollection>();

            var viewModel = new MoreJobsViewModel()
            {
                CompositeSettings = _compositeSettings.Value,
                SearchService = "searchService"
            };
            var settings = viewModel.CompositeSettings;
            var searchService = viewModel.SearchService;

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




            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public async Task WhenBody_Called_WithData_ReturnHtml()
        {
            var controller =
                new MoreJobsController(_dataProtectionProvider, _settings, _compositeSettings, _sessionService)
                {
                    ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()}
                };

            controller.HttpContext.Request.QueryString = QueryString.Create(".matchSkill-session", "Abc123");
            var requestCookie = new Mock<IRequestCookieCollection>();

            var viewModel = new MoreJobsViewModel()
            {
                CompositeSettings = _compositeSettings.Value,
                SearchService = "searchService"
            };
            var settings = viewModel.CompositeSettings;
            var searchService = viewModel.SearchService;

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

            var userSession = new UserSession()
            {
                Occupations = new HashSet<UsOccupation>(2)
                {
                    new UsOccupation("1", "FirstOccupation", DateTime.UtcNow),
                    new UsOccupation("2", "SecondOccupation", DateTime.UtcNow)
                }
            };
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(userSession);

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
