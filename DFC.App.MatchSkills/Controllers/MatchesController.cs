using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchesController : CompositeSessionController<MatchesCompositeViewModel>
    {
        public MatchesController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        public override async Task<IActionResult> Body()
        {
            var userSession = await GetUserSession();
            if (null != userSession)
            {
                foreach (var match in userSession.OccupationMatches)
                {
                    var cm = new CareerMatch()
                    {
                        JobSectorGrowthDescription = string.Empty,
                    };
                    cm.JobProfile.Title = match.JobProfileTitle;
                    cm.JobProfile.Description = "Job profile description will go here.";
                    cm.JobProfile.Url = match.JobProfileUri;
                    cm.MatchingEssentialSkills = match.MatchingEssentialSkills;
                    cm.MatchingOptionalSkills = match.MatchingOptionalSkills;
                    cm.TotalOccupationEssentialSkills = match.TotalOccupationEssentialSkills;
                    cm.TotalOccupationOptionalSkills = match.TotalOccupationOptionalSkills;
                    cm.SourceSkillCount = userSession.Skills.Count;
                    ViewModel.CareerMatches.Add(cm);
                }
            }

            return await base.Body();
        }
    }
}
