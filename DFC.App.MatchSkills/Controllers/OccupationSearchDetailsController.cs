using System;
using System.Diagnostics.CodeAnalysis;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    [ExcludeFromCodeCoverage]
    public class OccupationSearchDetailsController : CompositeSessionController<OccupationSearchDetailsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;

        public OccupationSearchDetailsController(IServiceTaxonomySearcher serviceTaxonomy,
            IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(compositeSettings,
                sessionService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
            _sessionService = sessionService;
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            return RedirectTo($"{CompositeViewModel.PageId.OccupationSearch}");
        }


        [SessionRequired]
        [HttpGet, HttpPost]
        [Route("body/OccupationSearchDetails")]
        public async Task<IActionResult> Body(string occupationSearchGovUkInputSearch)
        {
            await TrackPageInUserSession();
            ViewModel.Occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupationSearchGovUkInputSearch, bool.Parse(_settings.SearchOccupationInAltLabels));
            return await base.Body();
        }
    }
}
