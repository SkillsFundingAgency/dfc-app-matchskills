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
        
        [HttpPost]
        [Route("MatchSkills/[controller]")]
        public async Task<IActionResult> Body(WorkedBefore choice)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);
            var userWorkedBefore = choice == WorkedBefore.Undefined ? (bool?)null : choice == WorkedBefore.Yes;


            if (!string.IsNullOrWhiteSpace(primaryKeyFromCookie))
            {
                var createSessionRequest = new CreateSessionRequest()
                {
                    CurrentPage = CompositeViewModel.PageId.Worked.Value,
                    UserHasWorkedBefore = userWorkedBefore
                };
                await CreateUserSession( createSessionRequest, primaryKeyFromCookie);
            }
            
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
