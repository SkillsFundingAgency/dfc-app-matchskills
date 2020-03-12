using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
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
            ISessionService sessionService )
            : base(compositeSettings, sessionService )
        {
        }

        public override async Task<IActionResult> Body()
        {
            var createSessionRequest = new CreateSessionRequest()
            {
                CurrentPage = CompositeViewModel.PageId.Worked.Value
            };
            await CreateUserSession(createSessionRequest);
            
            ViewModel.HasError = HasErrors();

            return await base.Body();
        }

        [HttpPost]
        [SessionRequired]
        public async Task<IActionResult> Body(WorkedBefore choice)
        {
            var userWorkedBefore = choice == WorkedBefore.Undefined ? (bool?)null : choice == WorkedBefore.Yes;

            var session = await GetUserSession();
            session.UserHasWorkedBefore = userWorkedBefore;
            await UpdateUserSession(ViewModel.Id.Value, session);

            switch (choice)
            {
                case WorkedBefore.Yes:
                    return RedirectTo(CompositeViewModel.PageId.Route.Value);
                case WorkedBefore.No:
                    return RedirectTo(CompositeViewModel.PageId.Worked.Value);   // go to DYSAC question 1 (call new assessment)
                default:
                    return RedirectWithError(ViewModel.Id.Value);
            }
        }
    }
}
