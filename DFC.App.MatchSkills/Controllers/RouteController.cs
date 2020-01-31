using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class RouteController : CompositeSessionController<RouteCompositeViewModel>
    {
        private readonly CompositeSettings _compositeSettings;
        public RouteController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings) 
            : base(dataProtectionProvider, compositeSettings)
        {
            _compositeSettings = compositeSettings.Value;
        }

        
        [Route("MatchSkills/body/[controller]")]
        [HttpPost]
        public IActionResult Body(Route choice)
        {
            switch (choice)
            {
                case Route.Jobs:
                    return RedirectPermanent($"{_compositeSettings.Path}/{CompositeViewModel.PageId.OccupationSearch}");
                case Route.JobsAndSkills:
                    default:
                    return base.Body();
            }
        }
    }
}