using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class EnterSkillsController : CompositeSessionController<EnterSkillsCompositeViewModel>
    {
        public EnterSkillsController(IOptions<CompositeSettings> compositeSettings, ISessionService sessionService, ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
        }
        public override async Task<IActionResult> Body()
        {
            await TrackPageInUserSession();
            var userSession = await GetUserSession();
            ViewModel.Skills.LoadFrom(userSession);
            return await base.Body();

        }

   
    }
}