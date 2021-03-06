﻿using System.Collections.Generic;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchDetailsController : CompositeSessionController<MatchDetailsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;

        public MatchDetailsController(IServiceTaxonomySearcher serviceTaxonomy,
            IOptions<ServiceTaxonomySettings> settings, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService) : base(compositeSettings, sessionService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;

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

            ViewModel.MatchingSkills = GetSkillsCombined(skillsGap.MatchingSkills, skillsGap.MissingSkills);
            ViewModel.OptionalMatchingSkills = GetSkillsCombined(skillsGap.OptionalMatchingSkills, skillsGap.OptionalMissingSkills);
            ViewModel.CareerTitle = UpperCaseFirstLetter(skillsGap.CareerTitle);
            ViewModel.CareerDescription = skillsGap.CareerDescription;
            ViewModel.JobGrowth = skillsGap.JobGrowth;
            return await base.Body();
        }

        public async Task<SkillsGap> GetSkillsGap(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var userSession = await GetUserSession();


            var skillsList = userSession.Skills.Select(x => x.Id).ToArray();

            if (userSession.Skills == null || userSession.Skills.Count == 0)
                return null;


            var skillsGap = await _serviceTaxonomy.GetSkillsGapForOccupationAndGivenSkills<SkillsGap>(_settings.ApiUrl,
                _settings.ApiKey, id, skillsList);
            return skillsGap;
        }

        public string UpperCaseFirstLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return char.ToUpper(str[0]) + str.Substring(1);
        }

        private IOrderedEnumerable<KeyValuePair<string, bool>> GetSkillsCombined(string[] matchedSkills, string[] missingSkills)
        {
            var dict = new Dictionary<string, bool>();

            foreach (string matchedSkill in matchedSkills)
            {
                dict.Add(matchedSkill, true);
            }
            foreach (string unmatchedSkill in missingSkills)
            {
                dict.Add(unmatchedSkill, false);
            }

            return dict.OrderByDescending(x => x.Key);
        }
    }
}
