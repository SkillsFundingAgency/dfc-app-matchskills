
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class ReloadController : CompositeSessionController<ReloadCompositeViewModel>
    {
        public ReloadController(IOptions<CompositeSettings> compositeSettings, ISessionService sessionService) : base(
            compositeSettings, sessionService)
        {
        }

        public override async Task<IActionResult> Body()
        {
            var session = Request.Query["sessionId"];

            if (string.IsNullOrEmpty(session))
            {
                return RedirectTo("");
            }

            var userSession = await GetUserSession(session);

            return userSession == null ? RedirectWithError("home") : RedirectTo(GetRoute(userSession));
        }

        [HttpPost]
        public async Task<IActionResult> Body(string homeGovUkTextInputCode)
        {
            var userSession = await GetUserSession(homeGovUkTextInputCode);

            return userSession == null ? RedirectWithError("home"): RedirectTo(GetRoute(userSession));
        }

        private static string GetRoute(UserSession session)
        {
            if (session == null)
            {
                return CompositeViewModel.PageId.Home.Value;
            }

            if (session.UserHasWorkedBefore.HasValue && !session.UserHasWorkedBefore.Value)
            {
                return "DysacRoute";
            }
            else if (session.RouteIncludesDysac.HasValue && session.RouteIncludesDysac.Value)
            {
                return session.CurrentPage != CompositeViewModel.PageId.Route.Value ? session.CurrentPage : "DysacRoute";
            }

            return session.CurrentPage;
        }
    }
}
