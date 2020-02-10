using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class RouteController : CompositeSessionController<RouteCompositeViewModel>
    {
        public RouteController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService) 
            : base(dataProtectionProvider, compositeSettings, sessionService)
        {
        }


        [Route("MatchSkills/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Body(Route choice)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);

            await UpdateUserSession(primaryKeyFromCookie, CompositeViewModel.PageId.Route.Value);

            switch (choice)
            {
                case Route.Jobs:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.OccupationSearch}");
                case Route.JobsAndSkills:
                    return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Route}");
                default:
                    ViewModel.HasError = true;
                    return base.Body();
            }
        }
    }
}