using System;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : CompositeSessionController<OccupationSearchCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;
       
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
            _sessionService = sessionService;
        }


        public override async Task<IActionResult> Body()
        {
            ViewModel.SearchService = _settings.SearchService;

            await TrackPageInUserSession();

            return await base.Body();
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
        [Route("Matchskills/OccupationSearchAuto")]
        [Route("OccupationSearchAuto")]
        public async Task<IActionResult> OccupationSearchAuto(string occupation)
        {
            var occupations = await OccupationSearch(occupation);
           return this.Ok(occupations.Select(x => x.Name).ToList());
        }
        [HttpPost]
        [Route("/matchskills/occupationSearch/SearchSkills")]
        public async Task<IActionResult> SearchSkills(string enterJobInputAutocomplete)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);
            var resultGet = await _sessionService.GetUserSession(primaryKeyFromCookie);
            var occupationId = await GetOccupationIdFromName(enterJobInputAutocomplete);

            if (resultGet.Occupations == null)
            {
                resultGet.Occupations = new HashSet<UsOccupation>();
            }
            
            resultGet.Occupations.Add(new UsOccupation(occupationId, enterJobInputAutocomplete, DateTime.Now));
            await _sessionService.UpdateUserSessionAsync(resultGet);
            
            return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.SelectSkills}");
        }
        public async Task<string> GetOccupationIdFromName(string occupation)
        {
            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, bool.Parse(_settings.SearchOccupationInAltLabels));
            return occupations.Single(x => x.Name == occupation).Id;
        }
    }

}