using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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
            await LoadSkills();

            ViewModel.HasError = HasErrors();
            return await base.Body();

        }
        [HttpPost]
        [SessionRequired]
        [Route("MatchSkills/[controller]")]
        public IActionResult Body(string enterSkillsInputInput)
        {

            if (string.IsNullOrWhiteSpace(enterSkillsInputInput))
            {
                ViewModel.HasError = true;
                return RedirectWithError(ViewModel.Id.Value);
            }
            return RedirectTo($"{CompositeViewModel.PageId.RelatedSkills}?searchTerm={enterSkillsInputInput}");
        }

        public async Task LoadSkills()
        {
            await TrackPageInUserSession();
            var userSession = await GetUserSession();
            ViewModel.Skills.LoadFrom(userSession);
        }

    }
}