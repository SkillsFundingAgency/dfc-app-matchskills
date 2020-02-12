using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using  DFC.App.MatchSkills.Extensions;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : CompositeSessionController<OccupationSearchCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
       
        public OccupationSearchController(IDataProtectionProvider dataProtectionProvider,
            IServiceTaxonomySearcher serviceTaxonomy, 
            IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService) 
            : base(dataProtectionProvider, compositeSettings,
                sessionService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }


        public override IActionResult Body()
        {
            ViewModel.SearchService = _settings.SearchService;
            return base.Body();
        }

        [HttpGet,HttpPost]
        [Route("/OccupationSearch")]
        public async Task<IEnumerable<Occupation>> OccupationSearch(string occupation)
        {
            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, bool.Parse(_settings.SearchOccupationInAltLabels));

            return occupations.ToList();
        }

        [HttpGet,HttpPost]
        [Route("matchskills/OccupationSearchAuto")]
        public async Task<IActionResult> OccupationSearchAuto(string occupation)
        {
            var occupations = await OccupationSearch(occupation);
           // return this.NegotiateContentResult(occupations.Select(x => x.Name).ToList());
           return this.Ok(occupations.Select(x => x.Name).ToList());
        }

    }

}