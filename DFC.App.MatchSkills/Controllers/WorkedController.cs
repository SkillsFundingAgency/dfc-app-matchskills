using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Services;
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
            : base(dataProtectionProvider, compositeSettings)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        [Route("MatchSkills/body/[controller]")]
        public IActionResult Body(WorkedBefore choice)
        {
            var sessionId = _sessionService.CreateUserSession(CompositeViewModel.PageId.Home.Value,
                CompositeViewModel.PageId.Worked.Value).Result;

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                AppendCookie(sessionId);
            }
            switch (choice)
            {
                case WorkedBefore.Yes:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Route}");
                case WorkedBefore.No:
                default:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Worked}");
            }
        }
    }
}
