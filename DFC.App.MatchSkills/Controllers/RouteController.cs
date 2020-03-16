using System;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;

namespace DFC.App.MatchSkills.Controllers
{
  
    [SessionRequired]
    public class RouteController : CompositeSessionController<RouteCompositeViewModel>
    {
        private readonly IOptions<DysacSettings> _dysacSettings;
        private readonly IDysacSessionReader _dysacService;
        public RouteController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, IDysacSessionReader dysacService, 
            IOptions<DysacSettings> dysacSettings ) 
            : base(compositeSettings, sessionService )
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var session = await GetUserSession();
            
            ViewModel.RouteIncludesDysac = session.RouteIncludesDysac;
            ViewModel.HasError = HasErrors();
            return await base.Body();
        }

        [SessionRequired]
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
                    return RedirectTo(CompositeViewModel.PageId.OccupationSearch.Value);
                case Route.JobsAndSkills:
                    var response = _dysacService.InitiateDysac(userSession.UserSessionId).Result;
                    return response.ResponseCode == DysacReturnCode.Ok ? Redirect(_dysacSettings.Value.DysacUrl) :  throw new Exception(response.ResponseMessage);
                    
                default:
                    return RedirectWithError(ViewModel.Id.Value);
            }
        }
    }
}