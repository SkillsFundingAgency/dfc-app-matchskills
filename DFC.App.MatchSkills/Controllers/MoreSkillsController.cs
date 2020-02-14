using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class MoreSkillsController : CompositeSessionController<MoreSkillsCompositeViewModel>
    {
        public MoreSkillsController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService) 
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }

        [SessionRequired]
        [HttpPost]
        [Route("MatchSkills/[controller]")]
        public async Task<IActionResult> Body(MoreSkills choice)
        {
            await TrackPageInUserSession(await GetUserSession());

            switch (choice)
            {
                case MoreSkills.Job:
                    return RedirectPermanent($"{base.ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.MoreJobs}");
                case MoreSkills.Skill:
                    return RedirectPermanent($"{base.ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.MoreSkills}");
                default:
                    ViewModel.HasError = true;
                    return await base.Body();
            }
        }
    }
}
