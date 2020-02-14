using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class MoreSkillsController : CompositeSessionController<MoreSkillsCompositeViewModel>
    {
        public MoreSkillsController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService) 
            : base(dataProtectionProvider, compositeSettings, sessionService)
        {
        }

        [HttpPost]
        [Route("MatchSkills/[controller]")]
        public async Task<IActionResult> Body(MoreSkills choice)
        {
            switch (choice)
            {
                case MoreSkills.Job:
                    return RedirectPermanent($"{base.ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.MoreJobs}");
                case MoreSkills.Skill:
                    return RedirectPermanent($"{base.ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.EnterSkills}");
                default:
                    ViewModel.HasError = true;
                    return await base.Body();
            }
        }
    }
}
