using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class ConfirmRemoveController : CompositeSessionController<ConfirmRemoveCompositeViewModel>
    {
        public ConfirmRemoveController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var userSession = await GetUserSession();
            await TrackPageInUserSession(userSession);

            ViewModel.Skills.LoadSkillsToRemove(userSession);


            return await base.Body();
        }

        [HttpPost]
        [Route("/MatchSkills/[controller]")]
        [SessionRequired]
        public async Task<IActionResult> Body(IFormCollection formCollection)
        {
            var userSession = await GetUserSession();

            if (formCollection.Count == 0)
            {
                return RedirectWithError(CompositeViewModel.PageId.RemoveSkills.Value);
            }

            
            foreach (var key in formCollection.Keys)
            {
                string[] skill = key.Split("--");
                Throw.IfNull(skill[0], nameof(skill));
                Throw.IfNull(skill[1], nameof(skill));
                userSession.SkillsToRemove.Add(new UsSkill(skill[0], skill[1], DateTime.Now));
            }

            await UpdateUserSession(userSession.UserSessionId, ViewModel.Id.Value, userSession);

           return RedirectTo(ViewModel.Id.Value);
        }

        [Route("/body/[controller]/RemoveSkillsSessionClear")]
        [SessionRequired]
        public async Task<IActionResult> RemoveSkillsSessionClear()
        {
            var userSession = await GetUserSession();

            userSession.SkillsToRemove = new HashSet<UsSkill>();

            await UpdateUserSession(userSession.UserSessionId, ViewModel.Id.Value, userSession);

            return RedirectTo(CompositeViewModel.PageId.SkillsBasket.Value);
        }
    }
}
