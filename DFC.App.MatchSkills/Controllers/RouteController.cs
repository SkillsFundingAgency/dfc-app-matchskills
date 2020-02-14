using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    [SessionRequired]
    public class RouteController : CompositeSessionController<RouteCompositeViewModel>
    {
        public RouteController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService) 
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var session = await GetUserSession();
            
            ViewModel.RouteIncludesDysac = session.RouteIncludesDysac;

            return await base.Body();
        }

        [SessionRequired]
        [Route("MatchSkills/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Body(Route choice)
        {
            var routeIncludesDysac = choice == Route.Undefined ? (bool?)null : choice == Route.JobsAndSkills;
            var userSession = await GetUserSession();
            userSession.RouteIncludesDysac = routeIncludesDysac;
            await TrackPageInUserSession(userSession);

            switch (choice)
            {
                case Route.Jobs:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.OccupationSearch}");
                case Route.JobsAndSkills:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Route}");
                default:
                    ViewModel.HasError = true;
                    return await base.Body();
            }
        }
    }
}