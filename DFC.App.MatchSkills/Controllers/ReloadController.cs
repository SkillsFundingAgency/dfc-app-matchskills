using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class ReloadController : CompositeSessionController<ReloadCompositeViewModel>
    {
        private readonly DysacSettings _dysacSettings;
        private readonly IDysacSessionReader _dysacService;

        public ReloadController(IOptions<CompositeSettings> compositeSettings, ISessionService sessionService, IOptions<DysacSettings> dysacSettings, IDysacSessionReader dysacService) : base(
            compositeSettings, sessionService)
        {
            _dysacService = dysacService;
            _dysacSettings = dysacSettings.Value;
        }

        public override async Task<IActionResult> Body()
        {
            var session = Request.Query["sessionId"];

            if (string.IsNullOrEmpty(session))
            {
                return RedirectTo("");
            }

            return await RedirectToAssessmentOrErrorPage(session);
        }

        [HttpPost]
        public async Task<IActionResult> Body(string homeGovUkTextInputCode)
        {
            return string.IsNullOrEmpty(homeGovUkTextInputCode) ? RedirectWithError("home") 
                : await RedirectToAssessmentOrErrorPage(homeGovUkTextInputCode);
        }

        private async Task<IActionResult> RedirectToAssessmentOrErrorPage(string code)
        {
            var userSession = await GetUserSession(code);

            if (userSession != null)
            {
                return GetRouteForMatchSkillsUser(userSession);
            }

            var result = await _dysacService.LoadExistingDysacOnlyAssessment(GetSessionId(code));

            return result.ResponseCode == DysacReturnCode.Ok ? Redirect(_dysacSettings.DysacReturnUrl) : RedirectWithError("home");
        }

        private IActionResult GetRouteForMatchSkillsUser(UserSession session)
        {
            if (session.UserHasWorkedBefore.HasValue && !session.UserHasWorkedBefore.Value)
            {
                return Redirect(_dysacSettings.DysacReturnUrl);
            }
            else if (session.RouteIncludesDysac.HasValue && session.RouteIncludesDysac.Value)
            {
                return session.CurrentPage != CompositeViewModel.PageId.Route.Value ? RedirectTo(session.CurrentPage) : Redirect(_dysacSettings.DysacReturnUrl);
            }

            return RedirectTo(session.CurrentPage);
        }
    }
}
