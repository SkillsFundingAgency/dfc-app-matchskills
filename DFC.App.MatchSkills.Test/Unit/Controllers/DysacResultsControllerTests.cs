using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    public class DysacResultsControllerTests
    {
        private DysacResultsController _controller;
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        [OneTimeSetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession().ReturnsForAnyArgs(new UserSession());
            _controller = new DysacResultsController(_compositeSettings, _sessionService);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };


        }
        [Test]
        public async Task WhenBodyCalled_RedirectToOccupationSearch()
        {
            var result = await _controller.Body() as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"~/{CompositeViewModel.PageId.OccupationSearch}");
        }
        [Test]
        public async Task AssignValues()
        {
            var oldDysacSettings = new OldDysacSettings()
            {
                ApiKey = "key",
                AssessmentApiUrl = "Url",
                DysacResultsUrl = "url"
            };
            var key = oldDysacSettings.ApiKey;
            var assessment = oldDysacSettings.AssessmentApiUrl;
            var dysac = oldDysacSettings.DysacResultsUrl;
        }
    }
}
