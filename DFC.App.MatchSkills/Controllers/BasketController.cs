using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class BasketController : CompositeSessionController<SkillsBasketCompositeViewModel>
    {

        public BasketController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base( compositeSettings, sessionService)
        { }

        public override async Task<IActionResult> Body()
        {
            await TrackPageInUserSession();
            var userSession = await GetUserSession();
            ViewModel.Skills.LoadFrom(userSession);

            return await base.Body();
        }
    }
}
