using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class WorkedController : CompositeSessionController<WorkedCompositeViewModel>
    {

        public WorkedController(IDataProtectionProvider dataProtectionProvider,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(dataProtectionProvider, compositeSettings, sessionService)
        {
        }

        [HttpPost]
        [Route("MatchSkills/[controller]")]
        public async Task<IActionResult> Body(WorkedBefore choice)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);

            if (!string.IsNullOrWhiteSpace(primaryKeyFromCookie))
            {
                var createSessionRequest = new CreateSessionRequest()
                {
                    CurrentPage = CompositeViewModel.PageId.Worked.Value,
                    UserHasWorkedBefore = choice == WorkedBefore.Yes
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
                    return base.Body();
            }
        }
    }
}
