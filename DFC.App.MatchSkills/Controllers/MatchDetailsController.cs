using System;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Dfc.ProviderPortal.Packages;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchDetailsController : CompositeSessionController<MatchDetailsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;

        public MatchDetailsController(IServiceTaxonomySearcher serviceTaxonomy, 
            IOptions<ServiceTaxonomySettings> settings, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService) : base(compositeSettings, sessionService, cookieService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
            _sessionService = sessionService;

        }

        [SessionRequired]
        [HttpGet]
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }

        [SessionRequired]
        [HttpGet]
        [Route("/body/[controller]/{*id}")]
        public async Task<IActionResult> Body(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectPermanent(CompositeViewModel.PageId.Matches.Value);
            }

            var skillsGap = await GetSkillsGap(id);

            ViewModel.MissingSkills = skillsGap.MissingSkills;
            ViewModel.CareerDescription = skillsGap.CareerDescription;
            ViewModel.MatchingSkills = skillsGap.MatchingSkills; 
            ViewModel.CareerTitle = skillsGap.CareerTitle;

            return await base.Body();
        }

        public async Task<SkillsGap> GetSkillsGap(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var userSession = await GetUserSession();

            var occupation = userSession.Occupations.Where(x => x.Name.Contains(id)).Select(x => x.Name)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(occupation))
                return null;
            
            var skillsList = userSession.Skills.Select(x => x.Name).ToArray();

            if (userSession.Skills == null || userSession.Skills.Count == 0)
                return null;


            return await _serviceTaxonomy.GetSkillsGapForOccupationAndGivenSkills<SkillsGap>(_settings.ApiUrl,
                _settings.ApiKey, occupation, skillsList);
        }
    }
}
