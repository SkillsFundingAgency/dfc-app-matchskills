using Dfc.Session.Models;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

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
            _dysacService = dysacService;
            _dysacSettings = dysacSettings;
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var session = await GetUserSession();
            await TrackPageInUserSession(session);
            ViewModel.RouteIncludesDysac = session.RouteIncludesDysac;
            ViewModel.HasError = HasErrors();
            return await base.Body();
        }

        [SessionRequired]
        [HttpPost]
        public async Task<IActionResult> Body(Route choice)
        {
            var routeIncludesDysac = choice == Route.Undefined ? (bool?) null : choice == Route.JobsAndSkills;
            var userSession = await GetUserSession();
           
            switch (choice)
            {
                case Route.Jobs:
                    await UpdateUserSession(userSession, routeIncludesDysac);
                    return RedirectTo(CompositeViewModel.PageId.OccupationSearch.Value);
                case Route.JobsAndSkills:


                    _dysacService.InitiateDysac(new DfcUserSession()
                    {
                        CreatedDate = userSession.SessionCreatedDate,
                        PartitionKey = userSession.PartitionKey,
                        Salt = userSession.Salt,
                        SessionId = userSession.UserSessionId,
                        Origin = Origin.MatchSkills
                    });

                    await UpdateUserSession(userSession, routeIncludesDysac);
                    return Redirect(_dysacSettings.Value.DysacUrl);
            }



            return RedirectWithError(ViewModel.Id.Value);
        }

        private async Task<UserSession> UpdateUserSession(UserSession userSession,bool? routeIncludesDysac)
        {
            userSession.RouteIncludesDysac = routeIncludesDysac;
            await TrackPageInUserSession(userSession);
            return userSession;
        }

    }
}