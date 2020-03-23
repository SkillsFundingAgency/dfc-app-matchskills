using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Service;
using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.ViewComponents.Pagination;
using DFC.App.MatchSkills.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Test.Unit.Controllers
{
    [TestFixture]
    public class MatchesControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ISessionService _sessionService;
         
        private IOptions<PageSettings> _pageSettings;
        private IOptions<DysacSettings> _dysacSettigs;


        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _dysacSettigs = Options.Create(new DysacSettings());
            _pageSettings = Options.Create(new PageSettings());
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new MatchesController(_compositeSettings, _sessionService , _pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"page","0" }
            });

            _sessionService.GetUserSession().ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalledWithOutPage_ReturnCurrentPageAs1()
        {
            var controller = new MatchesController(_compositeSettings, _sessionService , _pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
            });

            _sessionService.GetUserSession().ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(1);
        }


        [Test]
        public async Task WhenTotalResultsIsGreaterThanPage_Then_CorrectTotalPagesNumberReturned()
        {
            var userSession = MockHelpers.GetUserSession(true, true, true);
            userSession.OccupationMatches.Add(new OccupationMatch()
            {
                JobProfileTitle = "Mock Title3",
                JobProfileUri = "http://mockjoburl",
                LastModified = DateTime.UtcNow,
                TotalOccupationEssentialSkills = 12,
                MatchingEssentialSkills = 6,
                TotalOccupationOptionalSkills = 4,
                MatchingOptionalSkills = 2,
                Uri = "MatchUri",
            }
            );

            var pageSettings = Options.Create(new PageSettings() { PageSize = 1 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
             {
                 {"page","1" }
             });

            _sessionService.GetUserSession()
                .ReturnsForAnyArgs(userSession);

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(1);
            result.ViewData.Model.As<MatchesCompositeViewModel>().TotalPages.Should().Be(3);
        }


        [Test]
        public async Task WhenTotalResultsMatchesPageSize_Then_CorrectTotalPagesNumberReturned()
        {
            var pageSettings = Options.Create(new PageSettings() { PageSize = 2 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
             {
                 {"page","2" }
             });

            _sessionService.GetUserSession()
                .ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(1);
        }


        [Test]
        public async Task WhenBodyCalledWithPageNumber_ReturnCorrectPage()
        {
            var pageSettings = Options.Create(new PageSettings() { PageSize = 1 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
             {
                 {"page","2" }
             });

            _sessionService.GetUserSession()
                .ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(2);
        }

        [Test]
        public async Task WhenBodyCalledWithPageNumberGreaterThanTheNumberOfPages__Then_ReturnLastPage()
        {
            var pageSettings = Options.Create(new PageSettings() { PageSize = 1 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
             {
                 {"page","10" }
             });

            _sessionService.GetUserSession()
                .ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(2);
        }

        [Test]
        public void When_SkillsExist_Then_MatchPercentageShouldBeCalculated()
        { // Arrange.

            var cm = new OccupationMatch();
            cm.MatchingEssentialSkills = 1;
            cm.MatchingOptionalSkills = 2;
            cm.TotalOccupationEssentialSkills = 10;
            cm.TotalOccupationOptionalSkills = 3;

            // Act.
            var result = cm.MatchStrengthPercentage;

            // Assert.
            result.Should().Be(10, because: "we matched one skill out of ten essential skills");
        }

        [Test]
        public void WhenPaginationModelCalled_ResultReturned()
        {
            var paginationViewModel = new PaginationViewModel()
            {
                NextPage = 1,
                TotalPages = 1,
                NextPageLink = "",
                PreviousPage = 2,
                PreviousPageLink = "",
                ShowResultsString = ""
            };
        }

        [Test]
        public async Task WhenTotalResultsIsGreaterThanPageAndModuleOfResultByPagesISLessThan5_Then_CorrectTotalPagesNumberReturned()
        {
            var userSession = MockHelpers.GetUserSession(true, true, true);

            var x = 1;

            while (x < 10)
            {


                userSession.OccupationMatches.Add(new OccupationMatch()
                {
                    JobProfileTitle = $"Mock Title{x}",
                    JobProfileUri = "http://mockjoburl",
                    LastModified = DateTime.UtcNow,
                    TotalOccupationEssentialSkills = 12,
                    MatchingEssentialSkills = 6,
                    TotalOccupationOptionalSkills = 4,
                    MatchingOptionalSkills = 2,
                    Uri = "MatchUri",
                }
                );
                x++;
            }
            var pageSettings = Options.Create(new PageSettings() { PageSize = 10 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
             {
                 {"page","1" }
             });

            _sessionService.GetUserSession()
                .ReturnsForAnyArgs(userSession);

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(1);
            result.ViewData.Model.As<MatchesCompositeViewModel>().TotalPages.Should().Be(2);
        }

        [TestCase("MatchPercentage", "ascending", "Mock Title2")]
        [TestCase("MatchPercentage", "descending", "Mock Title1")]
        [TestCase("Alphabetically", "ascending", "Mock Title1")]
        [TestCase("Alphabetically", "descending", "Mock Title2")]
        [TestCase("JobSectorGrowth", "ascending", "Mock Title2")]
        [TestCase("JobSectorGrowth", "descending", "Mock Title1")]
        public async Task WhenOrdering_Then_ReturnResultsInCorrectOrder(string sortBy, string direction, string expected)
        {
            var userSession = MockHelpers.GetUserSession(true, false, true);
            var match1 = "Mock Title1";
            var match2 = "Mock Title2";

            userSession.OccupationMatches.Add(new OccupationMatch()
            {
                JobProfileTitle = match1,
                JobProfileUri = "http://mockjoburl",
                LastModified = DateTime.UtcNow,
                JobGrowth = JobGrowth.Decreasing,
                TotalOccupationEssentialSkills = 12,
                MatchingEssentialSkills = 8,
                TotalOccupationOptionalSkills = 4,
                MatchingOptionalSkills = 2,
                Uri = "MatchUri",
            }
            );
            userSession.OccupationMatches.Add(new OccupationMatch()
            {
                JobProfileTitle = match2,
                JobProfileUri = "http://mockjoburl",
                LastModified = DateTime.UtcNow,
                JobGrowth = JobGrowth.Increasing,
                TotalOccupationEssentialSkills = 12,
                MatchingEssentialSkills = 6,
                TotalOccupationOptionalSkills = 4,
                MatchingOptionalSkills = 2,
                Uri = "MatchUri",
            }
            );


            var pageSettings = Options.Create(new PageSettings() { PageSize = 10 });
            var controller = new MatchesController(_compositeSettings, _sessionService, pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"page","1" },
                {"sortBy",sortBy },
                {"direction", direction }
            });

            _sessionService.GetUserSession()
                .ReturnsForAnyArgs(userSession);

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Model.As<MatchesCompositeViewModel>().CareerMatches.First().JobProfile.Title.Should()
                .Be(expected);
        }

        [Test]
        public async Task When_ChangingOrderType_Then_UpdateTheChoiceInSession()
        {
            var controller = new MatchesController(_compositeSettings, _sessionService, _pageSettings, _dysacSettigs);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"page","0" },
                {"sortBy","Alphabetically" },
                {"direction", "ascending" }
            });

            _sessionService.GetUserSession().ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Body() as ViewResult;

            await _sessionService.Received().UpdateUserSessionAsync(Arg.Is<UserSession>(x =>
               x.MatchesSortBy == SortBy.Alphabetically && x.MatchesSortDirection == SortDirection.Ascending));
        }
    }
}
