using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{

    public class WorkedController : CompositeSessionController<WorkedCompositeViewModel>
    {

        public WorkedController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
        }

        public override async Task<IActionResult> Body()
        {

            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);


            if (string.IsNullOrWhiteSpace(primaryKeyFromCookie))
            {
                var createSessionRequest = new CreateSessionRequest()
                {
                    CurrentPage = CompositeViewModel.PageId.Worked.Value
                };
                await CreateUserSession(createSessionRequest, primaryKeyFromCookie);
            }
            else
            {
                await TrackPageInUserSession();
                var session = await GetUserSession();
                ViewModel.HasWorkedBefore = session.UserHasWorkedBefore;
            }
            
            return await base.Body();
        }

        [HttpPost]
        [SessionRequired]
        [Route("MatchSkills/[controller]")]
        public async Task<IActionResult> Body(WorkedBefore choice)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);
            var userWorkedBefore = choice == WorkedBefore.Undefined ? (bool?)null : choice == WorkedBefore.Yes;

            var session = await GetUserSession();
            session.UserHasWorkedBefore = userWorkedBefore;
            await UpdateUserSession(primaryKeyFromCookie,
                ViewModel.Id.Value, session);


            switch (choice)
            {
                case WorkedBefore.Yes:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Route}");
                case WorkedBefore.No:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Worked}");
                default:
                    ViewModel.HasError = true;
                    return await base.Body();
            }
        }
    }
}
