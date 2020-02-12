using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class BasketController : CompositeSessionController<SkillsBasketCompositeViewModel>
    {
        public BasketController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(dataProtectionProvider, compositeSettings, sessionService)
        {
        }

        public override IActionResult Body()
        {
            TrackPageInUserSession();
            var userSession = GetUserSession();
            ViewModel.Skills.LoadFromSession(userSession);

            return base.Body();
        }
    }
}
