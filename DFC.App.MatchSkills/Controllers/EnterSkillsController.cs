using System.Text.Encodings.Web;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            return await base.Body();

        }
        [HttpPost]
        [SessionRequired]
        [Route("MatchSkills/[controller]")]
        public async Task<IActionResult> Body(string enterSkillsInputInput)
        {

            enterSkillsInputInput = System.Web.HttpUtility.UrlEncode(enterSkillsInputInput);

            if (string.IsNullOrWhiteSpace(enterSkillsInputInput))
            {
                ViewModel.HasError = true;
                await LoadSkills();
                return await base.Body();
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