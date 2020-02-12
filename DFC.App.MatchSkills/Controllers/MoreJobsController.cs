using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class MoreJobsController : CompositeSessionController<MoreJobsViewModel>
    {
        private readonly ServiceTaxonomySettings _settings;

        public MoreJobsController(IDataProtectionProvider dataProtectionProvider,
            IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(dataProtectionProvider, compositeSettings,
                sessionService)
        {
            Throw.IfNull(settings, nameof(settings));
            _settings = settings.Value;
        }


        public override IActionResult Body()
        {
            ViewModel.SearchService = _settings.SearchService;
            return base.Body();
        }
    }
}