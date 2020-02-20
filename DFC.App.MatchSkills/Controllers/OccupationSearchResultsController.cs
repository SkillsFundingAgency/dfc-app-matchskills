using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using Dfc.ProviderPortal.Packages;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchResultsController : CompositeSessionController<OccupationSearchResultsCompositeViewModel>
    {
        private const string PathName = "OccupationSearchResults";
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;
        public OccupationSearchResultsController(IServiceTaxonomySearcher serviceTaxonomy, 
            IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService) 
            : base(compositeSettings,
                sessionService, cookieService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
            _sessionService = sessionService;
        }

       
    }
}