using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.ViewModels.OccupationSearch;
using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : CompositeSessionController<OccupationSearchCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;

       
        public OccupationSearchController(IDataProtectionProvider dataProtectionProvider,
            IServiceTaxonomySearcher serviceTaxonomy, 
            IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings)
            : base(dataProtectionProvider,compositeSettings)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }

        /*
        [HttpGet,HttpPost]
        [Route("/body")]
        public async Task<IEnumerable<Occupation>> OccupationSearch(string occupation)
        {
            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, bool.Parse(_settings.SearchOccupationInAltLabels));

            return occupations.ToList();
        }

        [HttpGet,HttpPost]
        [Route("/OccupationSearchAuto")]
        public async Task<IEnumerable> OccupationSearchAuto(string occupation)
        {
            var occupations = await OccupationSearch(occupation);
            return occupations.Select(x =>x.Name).ToList();
        }
        */
    }
}