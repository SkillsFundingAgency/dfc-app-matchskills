using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Personalisation.Common.Net.RestClient;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : CompositeSessionController<OccupationSearchCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;
       
        public OccupationSearchController(IServiceTaxonomySearcher serviceTaxonomy, 
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

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            ViewModel.SearchService = _settings.SearchService;

            await TrackPageInUserSession();

            return await base.Body();
        }

        [SessionRequired]
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
            
            if (!occupations.Any())
                return NoContent();
            
            return this.Ok(occupations.Select(x => x.Name).ToList());
        }
        [HttpPost]
        [Route("/matchskills/occupationSearch/GetSkillsForOccupation")]
        public async Task<IActionResult> GetSkillsForOccupation(string enterJobInputAutocomplete)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);
            var resultGet = await _sessionService.GetUserSession(primaryKeyFromCookie);
            var occupationId = await GetOccupationIdFromName(enterJobInputAutocomplete);

            if (resultGet.Occupations == null)
            {
                resultGet.Occupations = new HashSet<UsOccupation>();
            }
            if (!resultGet.Occupations.Any(o => o.Id == occupationId))
                resultGet.Occupations.Add(new UsOccupation(occupationId, enterJobInputAutocomplete));
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