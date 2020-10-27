
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
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
    public class RemoveSkillsControllerTests
    {

        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
         

        [SetUp]
        public void Init()
        {
            _compositeSettings = Options.Create(new CompositeSettings());
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.GetUserSession().ReturnsForAnyArgs(new UserSession());

             

        }

        [Test]
        public async Task When_RemoveMoveSkillsCalled_ReturnView()
        {
            var controller = new RemoveSkillsController(_compositeSettings, _sessionService );
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
        public async Task When_RemoveMoveSkillsCalled_Then_InstantiateSkillsToRemove()
        {
            var controller = new RemoveSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.Body() as ViewResult;
            var vm = result.ViewData.Model as RemoveSkillsCompositeViewModel;

            vm.Skills.Count.Should().Be(0);
        }


        [Test]
        public async Task When_RemoveMoveSkillsCalled_TrackTrackPageInUserSession()
        {
            var controller = new RemoveSkillsController(_compositeSettings, _sessionService );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var result = await controller.Body() as ViewResult;

            await _sessionService.Received()
                .UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
                    x.CurrentPage == CompositeViewModel.PageId.RemoveSkills.Value));
        }
    }
}
