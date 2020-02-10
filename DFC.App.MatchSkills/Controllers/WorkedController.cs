using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class WorkedController : CompositeSessionController<WorkedCompositeViewModel>
    {
        private readonly ISessionService _sessionService;

        public WorkedController(IDataProtectionProvider dataProtectionProvider,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(dataProtectionProvider, compositeSettings, sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        [Route("MatchSkills/body/[controller]")]
        public async Task<IActionResult> Body(WorkedBefore choice)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);

            var primaryKey = await _sessionService.CreateUserSession(CompositeViewModel.PageId.Home.Value,
                CompositeViewModel.PageId.Worked.Value, primaryKeyFromCookie);

            if (string.IsNullOrWhiteSpace(primaryKeyFromCookie))
            {
                AppendCookie(primaryKey);
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
