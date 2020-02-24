using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Service;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Test.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class MatchDetailsControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private ICookieService _cookieService;
        private IServiceTaxonomySearcher _serviceTaxonomy;
        private IOptions<ServiceTaxonomySettings> _serviceTaxonomySettings;
        private MatchDetailsController _controller;


        [SetUp]
        public void Init()
        {

            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _cookieService = Substitute.For<ICookieService>();
            _sessionService.GetUserSession(Arg.Any<string>()).ReturnsForAnyArgs(new UserSession());
            _serviceTaxonomy = new ServiceTaxonomyRepository();
            _serviceTaxonomySettings = Options.Create(new ServiceTaxonomySettings());
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService, _cookieService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var userSession = new UserSession
            {
                Occupations = new HashSet<UsOccupation>(1)
                {
                    new UsOccupation("id", "id", DateTime.UtcNow)
                },
                Skills = new HashSet<UsSkill>(1)
                {
                    new UsSkill("id", "id", DateTime.UtcNow)
                }
            };

            _sessionService.GetUserSession(Arg.Any<string>()).Returns(userSession);
            _serviceTaxonomy = Substitute.For<IServiceTaxonomySearcher>();
            _serviceTaxonomy.GetSkillsGapForOccupationAndGivenSkills<SkillsGap>(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string[]>()).Returns(new SkillsGap());
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService, _cookieService);

            var result = await _controller.Body("id") as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }


    }
    public class GetSkillsGapTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
        private ICookieService _cookieService;
        private IServiceTaxonomySearcher _serviceTaxonomy;
        private IOptions<ServiceTaxonomySettings> _serviceTaxonomySettings;
        private MatchDetailsController _controller;


        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _cookieService = new CookieService(new EphemeralDataProtectionProvider());
            _serviceTaxonomy = new ServiceTaxonomyRepository();
            _serviceTaxonomySettings = Options.Create(new ServiceTaxonomySettings());
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService, _cookieService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

        }
        [Test]
        public async Task If_IdIsNull_ReturnNull()
        {
            var skillsGap = await _controller.GetSkillsGap("");
            skillsGap.Should().BeNull();

        }
        [Test]
        public async Task If_SkillsAreEmpty_ReturnNull()
        {
            var userSession = new UserSession
            {
                Occupations = new HashSet<UsOccupation>(1)
                {
                    new UsOccupation("id", "id", DateTime.UtcNow)
                }
            };
            _sessionService.GetUserSession(Arg.Any<string>()).Returns(userSession);
            var skillsGap = await _controller.GetSkillsGap("id");
            skillsGap.Should().BeNull();
        }
    }

}
