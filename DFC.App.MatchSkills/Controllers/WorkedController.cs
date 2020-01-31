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
            IOptions<CompositeSettings> compositeSettings)
            : base(dataProtectionProvider, compositeSettings)
        {
        }

        [HttpPost]
        [Route("MatchSkills/body/[controller]")]
        public IActionResult Body(WorkedBefore choice)
        {
            switch (choice)
            {
                case WorkedBefore.Yes:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Route}");
                case WorkedBefore.No:
                default:
                    return base.Body();
            }
        }
    }
}
