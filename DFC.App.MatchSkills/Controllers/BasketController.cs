using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class BasketController : CompositeSessionController<SkillsBasketCompositeViewModel>
    {
        public BasketController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(dataProtectionProvider, compositeSettings, sessionService)
        {
        }

        public override async Task<IActionResult> Body()
        {
            await TrackPageInUserSession();
            var userSession = await GetUserSession();
            ViewModel.Skills.LoadFromSession(userSession);

            return await base.Body();
        }
    }
}
