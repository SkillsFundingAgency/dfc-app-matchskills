using System;
using System.Collections.Generic;
using System.Linq;
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
using DFC.App.MatchSkills.ViewModels;
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
         
        private IServiceTaxonomySearcher _serviceTaxonomy;
        private IOptions<ServiceTaxonomySettings> _serviceTaxonomySettings;
        private MatchDetailsController _controller;


        [SetUp]
        public void Init()
        {

            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
             
            _sessionService.GetUserSession().ReturnsForAnyArgs(new UserSession());
            _serviceTaxonomy = new ServiceTaxonomyRepository();
            _serviceTaxonomySettings = Options.Create(new ServiceTaxonomySettings());
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService );
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }
        [Test]
        public async Task WhenBaseBodyCalled_ReturnHtml()
        {
            var result = await _controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var userSession = new UserSession
            {
                Occupations = new HashSet<UsOccupation>(1)
                {
                    new UsOccupation("id", "id")
                },
                Skills = new HashSet<UsSkill>(1)
                {
                    new UsSkill("id", "id")
                },
                OccupationMatches = new List<OccupationMatch>()
                {
                    new OccupationMatch()
                    {
                        Uri = "id",
                        JobProfileUri = "id"
                    }
                }
            };

            _sessionService.GetUserSession().Returns(userSession);
            _serviceTaxonomy = Substitute.For<IServiceTaxonomySearcher>();
            _serviceTaxonomy.GetSkillsGapForOccupationAndGivenSkills<SkillsGap>(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string[]>()).Returns(new SkillsGap(){});
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService );

            var result = await _controller.Body("id") as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }


        [Test]
        public async Task WhenBodyCalled_ReturnHtmlWithSkills()
        {
            var userSession = new UserSession
            {
                Occupations = new HashSet<UsOccupation>(1)
                {
                    new UsOccupation("id", "id")
                },
                Skills = new HashSet<UsSkill>(1)
                {
                    new UsSkill("id", "id")
                },
                OccupationMatches = new List<OccupationMatch>()
                {
                    new OccupationMatch()
                    {
                        Uri = "id",
                        JobProfileUri = "id"
                    }
                }
            };

            _sessionService.GetUserSession().Returns(userSession);
            _serviceTaxonomy = Substitute.For<IServiceTaxonomySearcher>();
            _serviceTaxonomy.GetSkillsGapForOccupationAndGivenSkills<SkillsGap>(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string[]>()).Returns(new SkillsGap() { MatchingSkills = new []{"test"}, MissingSkills = new[] { "test2" }, OptionalMatchingSkills = new []{"test"}, OptionalMissingSkills = new[] { "test2" } });
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService);

            var result = await _controller.Body("id") as ViewResult;
            var model = result.Model as MatchDetailsCompositeViewModel;

            model.MatchingSkills.Count(x => x.Value == false && x.Key == "test2").Should().BeGreaterOrEqualTo(1);
            model.MatchingSkills.Count(x => x.Value && x.Key == "test").Should().BeGreaterOrEqualTo(1);
            model.OptionalMatchingSkills.Count(x => x.Value == false && x.Key == "test2").Should().BeGreaterOrEqualTo(1);
            model.OptionalMatchingSkills.Count(x => x.Value && x.Key == "test").Should().BeGreaterOrEqualTo(1);
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyWithEmptyIdCalled_ReturnMatches()
        {
            var result = await _controller.Body("") as RedirectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectResult>();
            result.Url.Should().Be($"{CompositeViewModel.PageId.Matches}");
        }

        [Test]
        public void When_UpperCaseFirstLetterCalled_ReturnCorrectValue()
        {
            var input = "this is a string";
            var result = _controller.UpperCaseFirstLetter(input);
            result.Should().Be("This is a string");
        }
    }
    public class GetSkillsGapTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
         
        private IServiceTaxonomySearcher _serviceTaxonomy;
        private IOptions<ServiceTaxonomySettings> _serviceTaxonomySettings;
        private MatchDetailsController _controller;


        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
             
            _serviceTaxonomy = new ServiceTaxonomyRepository();
            _serviceTaxonomySettings = Options.Create(new ServiceTaxonomySettings());
            _controller = new MatchDetailsController(_serviceTaxonomy, _serviceTaxonomySettings, _compositeSettings, _sessionService );
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
                    new UsOccupation("id", "id")
                }
            };
            _sessionService.GetUserSession().Returns(userSession);
            var skillsGap = await _controller.GetSkillsGap("id");
            skillsGap.Should().BeNull();
        }
    }

}
