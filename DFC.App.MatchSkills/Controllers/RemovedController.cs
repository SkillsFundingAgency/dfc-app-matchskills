﻿using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class RemovedController : CompositeSessionController<RemovedCompositeViewModel>
    {
        public RemovedController(IOptions<CompositeSettings> compositeSettings, ISessionService sessionService,
            ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var userSession = await GetUserSession();

            ViewModel.Skills.LoadSkillsToRemove(userSession);
            userSession.SkillsToRemove = new HashSet<UsSkill>();
            await UpdateUserSession(userSession.UserSessionId, ViewModel.Id.Value, userSession);
            return await base.Body();
        }

        [HttpPost]
        [Route("/MatchSkills/[controller]")]
        [SessionRequired]
        public async Task<IActionResult> Body(IFormCollection formCollection)
        {

            if (formCollection.Count == 0)
            {
                return RedirectWithError(CompositeViewModel.PageId.RemoveSkills.Value);
            }

            var userSession = await GetUserSession();

            foreach (var key in formCollection.Keys)
            {
                string[] skill = key.Split("--");
                Throw.IfNull(skill[0], nameof(skill));
                Throw.IfNull(skill[1], nameof(skill));
                userSession.Skills.Remove(userSession.Skills.FirstOrDefault(x=>x.Id == skill[0]));
            }

            await UpdateUserSession(userSession.UserSessionId, ViewModel.Id.Value, userSession);
            
            return RedirectTo(ViewModel.Id.Value);
        }
    }
}