using System.Collections.Generic;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Controllers
{
    public class RemoveSkillsController : CompositeSessionController<RemoveSkillsCompositeViewModel>
    {
        public RemoveSkillsController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            ViewModel.HasError = HasErrors();

            var userSession = await GetUserSession();
            userSession.SkillsToRemove = new HashSet<UsSkill>();
            await TrackPageInUserSession(userSession);
            ViewModel.Skills.LoadFrom(userSession);
            return await base.Body();
        }
        
    }

}
