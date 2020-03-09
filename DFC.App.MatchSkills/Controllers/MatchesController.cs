using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.LMI.Models;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchesController : CompositeSessionController<MatchesCompositeViewModel>
    {
        private readonly int pageSize;

        public MatchesController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService,   IOptions<PageSettings> pageSettings)
            : base(compositeSettings, sessionService)
        {
            pageSize = pageSettings.Value.PageSize;
        }

        public override async Task<IActionResult> Body()
        {
            var pageString = Request.Query["page"];
            if (!int.TryParse(pageString, out var page) || page == 0)
            {
                page = 1;
            }


            var totalMatches = 0;

            var userSession = await GetUserSession();
            if (null != userSession)
            {
                totalMatches = userSession.OccupationMatches.Count(m => m.MatchingEssentialSkills > 0);
                
                ViewModel.TotalPages = GetTotalPages(pageSize, totalMatches);

                if (page > ViewModel.TotalPages)
                {
                    page = ViewModel.TotalPages;
                }
                var showLmiData = userSession.OccupationMatches.All(x => x.JobGrowth != JobGrowth.Undefined);
                var skip = page > 1 ? (page -1) * pageSize : 0;

                foreach (var match in userSession.OccupationMatches.Where(m => m.MatchingEssentialSkills > 0)
                    .OrderByDescending(x => x.MatchStrengthPercentage).Skip(skip).Take(pageSize))
                {
                    var cm = new CareerMatch();
                    cm.JobProfile.Title = match.JobProfileTitle;
                    cm.JobProfile.Description = match.JobProfileDescription;
                    cm.JobProfile.Url = match.JobProfileUri;
                    cm.JobSectorGrowthDescription = match.JobGrowth;
                    cm.MatchingEssentialSkills = match.MatchingEssentialSkills;
                    cm.MatchingOptionalSkills = match.MatchingOptionalSkills;
                    cm.TotalOccupationEssentialSkills = match.TotalOccupationEssentialSkills;
                    cm.TotalOccupationOptionalSkills = match.TotalOccupationOptionalSkills;
                    cm.SourceSkillCount = userSession.Skills.Count;
                    cm.MatchStrengthPercentage = match.MatchStrengthPercentage;
                    cm.ShowLmiData = showLmiData;
                    ViewModel.CareerMatches.Add(cm);
                }
            }
            
            ViewModel.CurrentPage = page;
            var startValue = page > 1 ? pageSize * (page-1) + 1 : 1;
            var endValue = totalMatches < startValue + pageSize
                ? totalMatches
                : startValue + pageSize - 1;
            ViewModel.ResultsString = $"Showing {startValue}-{endValue} of {totalMatches} results";
            ViewModel.TotalMatches = totalMatches;
            return await base.Body();
        }

        private int GetTotalPages(int pageSize, int totalResults)
        {
            if (totalResults < 1 || totalResults <= pageSize)
            {
                return 1;
            }

            if (totalResults % pageSize > 1 && totalResults % pageSize < 5)
            {
                return (totalResults / pageSize) + 1;
            }

            return totalResults / pageSize;
        }
    }
}
