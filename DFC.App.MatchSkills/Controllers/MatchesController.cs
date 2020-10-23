using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchesController : CompositeSessionController<MatchesCompositeViewModel>
    {
        private readonly int _pageSize;
        private readonly IOptions<CompositeSettings> _compositeSettings;
        private readonly ServiceTaxonomySettings _serviceTaxonomySettings;
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;

        public MatchesController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, IOptions<PageSettings> pageSettings, IOptions<DysacSettings> dysacSettings, IOptions<ServiceTaxonomySettings> serviceTaxonomySettings, IServiceTaxonomySearcher serviceTaxonomy)
            : base(compositeSettings, sessionService)
        {
            _pageSize = pageSettings.Value.PageSize;
            _compositeSettings = compositeSettings;
            _serviceTaxonomy = serviceTaxonomy;
            _serviceTaxonomySettings = serviceTaxonomySettings.Value;
            ViewModel.DysacSaveUrl = dysacSettings.Value.DysacSaveUrl;
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {

            var userSession = await GetUserSession();

            if (userSession.Skills.Count == 0) return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.SkillsBasket}");

            await TrackPageInUserSession(userSession);
            await SetViewModel(userSession);

            return await base.Body();
        }

        [SessionRequired]
        [HttpPost]
        [Route("body/Matches")]
        public async Task<IActionResult> Submit()
        {

            var userSession = await GetUserSession();
            if (userSession.Skills.Count == 0) return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.SkillsBasket}");
            await TrackPageInUserSession(userSession);
            await SetViewModel(userSession);
            return await Task.FromResult<IActionResult>(View("Body",ViewModel));
        }

        private int GetTotalPages(int totalResults)
        {
            if (totalResults < 1 || totalResults <= _pageSize)
            {
                return 1;
            }

            if (totalResults % _pageSize > 0 && totalResults % _pageSize < 5)
            {
                return (totalResults / _pageSize) + 1;
            }

            return (int)Math.Round((decimal)totalResults / _pageSize);
        }

        private async Task SetViewModel(UserSession userSession)
        {
            int minimumMatch = Math.Min(_serviceTaxonomySettings.MinimumMatchingSkills, userSession.Skills.Count);

            var skillIds = userSession.Skills.Select(skill => skill.Id).ToArray();
            var matches = await _serviceTaxonomy.FindOccupationsForSkills(_serviceTaxonomySettings.ApiUrl, _serviceTaxonomySettings.ApiKey, skillIds, minimumMatch);

            var filters = GetFilters(userSession);

            var totalMatches = matches.Count();

            ViewModel.TotalPages = GetTotalPages(totalMatches);

            if (filters.Page > ViewModel.TotalPages)
            {
                filters.Page = ViewModel.TotalPages;
            }
            var skip = filters.Page > 1 ? (filters.Page - 1) * _pageSize : 0;

            ViewModel.CareerMatches = new List<CareerMatch>();
            foreach (var match in (GetOccupationMatches(matches, filters)).Skip(skip).Take(_pageSize))
            {
                var cm = new CareerMatch(_compositeSettings)
                {
                    JobProfile =
                    {
                        Title = match.JobProfileTitle,
                        Description = match.JobProfileDescription,
                        Url = match.JobProfileUri
                    },
                    JobSectorGrowthDescription = match.JobGrowth,
                    MatchingEssentialSkills = match.MatchingEssentialSkills,
                    MatchingOptionalSkills = match.MatchingOptionalSkills,
                    TotalOccupationEssentialSkills = match.TotalOccupationEssentialSkills,
                    TotalOccupationOptionalSkills = match.TotalOccupationOptionalSkills,
                    SourceSkillCount = userSession.Skills.Count,
                    MatchStrengthPercentage = match.MatchStrengthPercentage
                };
                ViewModel.CareerMatches.Add(cm);
            }

            ViewModel.CurrentPage = filters.Page;
            var startValue = filters.Page > 1 ? _pageSize * (filters.Page - 1) + 1 : 1;
            var endValue = totalMatches < startValue + _pageSize
                ? totalMatches
                : startValue + _pageSize - 1;
            ViewModel.ResultsString = $"Showing {startValue}-{endValue} of {totalMatches} results";
            ViewModel.TotalMatches = totalMatches;
            ViewModel.CurrentSortBy = filters.SortBy;
            ViewModel.CurrentDirection = filters.SortDirection;

        }

        private MatchesFilterModel GetFilters(UserSession userSession)
        {
            var pageString = Request.Query["page"];
            var sortByString = Request.Query["sortBy"];
            var sortDirectionString = Request.Query["direction"];

            if (!int.TryParse(pageString, out var page) || page == 0)
            {
                page = 1;
            }

            if (string.IsNullOrEmpty(sortByString) ||
                !Enum.TryParse(typeof(SortBy), sortByString, true, out var sortBy))
            {
                sortBy = userSession.MatchesSortBy;
            }

            if (string.IsNullOrEmpty(sortDirectionString) || !Enum.TryParse(typeof(SortDirection), sortDirectionString,
                true, out var sortDirection))
            {
                sortDirection = userSession.MatchesSortDirection;
            }

            userSession.MatchesSortBy = (SortBy)sortBy;
            userSession.MatchesSortDirection = (SortDirection)sortDirection;

            return new MatchesFilterModel
            {
                Page = page,
                SortDirection = (SortDirection)sortDirection,
                SortBy = (SortBy)sortBy
            };
        }

        private IEnumerable<OccupationMatch> GetOccupationMatches(OccupationMatch[] matches, MatchesFilterModel filters)
        {
            //var matchQuery= userSession.OccupationMatches.Where(m => m.MatchingEssentialSkills > 0);
            var matchQuery = matches;

            if (filters.SortDirection != SortDirection.Descending)
            {
                return filters.SortBy switch
                {
                    SortBy.Alphabetically => matchQuery.OrderBy(x => x.JobProfileTitle),
                    SortBy.JobSectorGrowth => matchQuery.OrderBy(x => x.JobGrowthSort)
                        .ThenBy(x => x.MatchStrengthPercentage)
                        .ThenBy(x => x.JobProfileTitle),
                    _ => matchQuery.OrderBy(x => x.MatchStrengthPercentage).ThenBy(x => x.JobProfileTitle)
                };
            }

            return filters.SortBy switch
            {
                SortBy.Alphabetically => matchQuery.OrderByDescending(x => x.JobProfileTitle),
                SortBy.JobSectorGrowth => matchQuery.OrderByDescending(x => x.JobGrowthSort)
                    .ThenBy(x => x.MatchStrengthPercentage)
                    .ThenBy(x => x.JobProfileTitle),
                _ => matchQuery.OrderByDescending(x => x.MatchStrengthPercentage).ThenBy(x => x.JobProfileTitle)
            };

        }
    }
}
