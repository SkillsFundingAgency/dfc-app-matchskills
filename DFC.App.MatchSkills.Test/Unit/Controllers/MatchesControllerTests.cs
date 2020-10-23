using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Test.Helpers;
using DFC.App.MatchSkills.ViewComponents.Pagination;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
        private IOptions<ServiceTaxonomySettings> _serviceTaxSettings;
        private IServiceTaxonomySearcher _serviceTaxonomy;
        const string SkillsJson = "{\"occupations\": [{\"uri\": \"http://data.europa.eu/esco/occupation/114e1eff-215e-47df-8e10-45a5b72f8197\",\"occupation\": \"renewable energy consultant\",\"alternativeLabels\": [\"alt 1\"],\"lastModified\": \"03-12-2019 00:00:01\"}]}";

        [SetUp]
        public void Init()
        {
            _sessionService = Substitute.For<ISessionService>();
            _serviceTaxonomy = Substitute.For<IServiceTaxonomySearcher>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _dysacSettigs = Options.Create(new DysacSettings());
            _pageSettings = Options.Create(new PageSettings());
            _serviceTaxSettings = Options.Create(new ServiceTaxonomySettings
            {
                ApiKey = "test",
                ApiUrl = "https://www.api.com"
            });

            var handlerMock = MockHelpers.GetMockMessageHandler(SkillsJson);
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomy = new ServiceTaxonomyRepository(restClient);
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new MatchesController(_compositeSettings, _sessionService , _pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
            var controller = new MatchesController(_compositeSettings, _sessionService , _pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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

            var x = 1;
            var occs = new GetOccupationsWithMatchingSkillsResponse
            {
                MatchingOccupations = new List<GetOccupationsWithMatchingSkillsResponse.MatchedOccupation>()
            };
            while (x < 4)
            {


                occs.MatchingOccupations.Add(new GetOccupationsWithMatchingSkillsResponse.MatchedOccupation()
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

            var handlerMock = MockHelpers.GetMockMessageHandler(JsonConvert.SerializeObject(occs));
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomy = new ServiceTaxonomyRepository(restClient);

            var pageSettings = Options.Create(new PageSettings() { PageSize = 1 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
        public async Task WhenBodyCalledWithPageNumberGreaterThanTheNumberOfPages__Then_ReturnLastPage()
        {
            var pageSettings = Options.Create(new PageSettings() { PageSize = 1 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
            result.ViewData.Model.As<MatchesCompositeViewModel>().CurrentPage.Should().Be(1);
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
            var occs = new GetOccupationsWithMatchingSkillsResponse
            {
                MatchingOccupations = new List<GetOccupationsWithMatchingSkillsResponse.MatchedOccupation>()
            };
            while (x < 10)
            {


                occs.MatchingOccupations.Add(new GetOccupationsWithMatchingSkillsResponse.MatchedOccupation()
                {
                     JobProfileTitle= $"Mock Title{x}",
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

            var handlerMock = MockHelpers.GetMockMessageHandler(JsonConvert.SerializeObject(occs));
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomy = new ServiceTaxonomyRepository(restClient);


            var pageSettings = Options.Create(new PageSettings() { PageSize = 5 });
            var controller = new MatchesController(_compositeSettings, _sessionService , pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
        public async Task WhenOrdering_Then_ReturnResultsInCorrectOrder(string sortBy, string direction, string expected)
        {
            var userSession = MockHelpers.GetUserSession(true, false, true);
            var match1 = "Mock Title1";
            var match2 = "Mock Title2";

            var occs = new GetOccupationsWithMatchingSkillsResponse
            {
                MatchingOccupations = new List<GetOccupationsWithMatchingSkillsResponse.MatchedOccupation>()
            };

            occs.MatchingOccupations.Add(new GetOccupationsWithMatchingSkillsResponse.MatchedOccupation
            {
                JobProfileTitle = match1,
                JobProfileUri = "http://mockjoburl",
                LastModified = DateTime.UtcNow,
                TotalOccupationEssentialSkills = 12,
                MatchingEssentialSkills = 8,
                TotalOccupationOptionalSkills = 4,
                MatchingOptionalSkills = 2,
                Uri = "MatchUri",
            }
            );
            occs.MatchingOccupations.Add(new GetOccupationsWithMatchingSkillsResponse.MatchedOccupation
            {
                JobProfileTitle = match2,
                JobProfileUri = "http://mockjoburl",
                LastModified = DateTime.UtcNow,
                TotalOccupationEssentialSkills = 12,
                MatchingEssentialSkills = 6,
                TotalOccupationOptionalSkills = 4,
                MatchingOptionalSkills = 2,
                Uri = "MatchUri",
            }
            );


            var handlerMock = MockHelpers.GetMockMessageHandler(JsonConvert.SerializeObject(occs));
            var restClient = new RestClient(handlerMock.Object);
            _serviceTaxonomy = new ServiceTaxonomyRepository(restClient);

            var pageSettings = Options.Create(new PageSettings() { PageSize = 10 });
            var controller = new MatchesController(_compositeSettings, _sessionService, pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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
            var controller = new MatchesController(_compositeSettings, _sessionService, _pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
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

        [Test]
        public async Task WhenSubmitCalled_ReturnHtml()
        {
            var controller = new MatchesController(_compositeSettings, _sessionService, _pageSettings, _dysacSettigs, _serviceTaxSettings, _serviceTaxonomy);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"page","0" }
            });

            _sessionService.GetUserSession().ReturnsForAnyArgs(MockHelpers.GetUserSession(true, true, true));

            var result = await controller.Submit() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
        }

    }
}
