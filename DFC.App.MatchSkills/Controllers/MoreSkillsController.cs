using DFC.App.MatchSkills.Application.Session.Interfaces;
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
            ISessionService sessionService ) 
            : base(compositeSettings, sessionService )
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
           await TrackPageInUserSession();
           ViewModel.HasError = HasErrors();
            return await base.Body();
        }

        [SessionRequired]
        [HttpPost]
        [Route("body/moreskills")]
        public IActionResult Body(MoreSkills choice)
        {
            switch (choice)
            {
                case MoreSkills.Job:
                    return RedirectTo(CompositeViewModel.PageId.MoreJobs.Value);
                case MoreSkills.Skill:
                    return RedirectTo(CompositeViewModel.PageId.EnterSkills.Value);
                default:
                    return RedirectWithError(ViewModel.Id.Value);
            }
        }
    }
}
