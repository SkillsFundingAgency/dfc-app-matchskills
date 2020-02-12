﻿using System;
using System.Collections.Generic;
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
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using Microsoft.AspNetCore.Http;

namespace DFC.App.MatchSkills.Controllers
{
    public class SelectSkillsController :  CompositeSessionController<SelectSkillsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly ISessionService _sessionService;
        public SelectSkillsController(IDataProtectionProvider dataProtectionProvider, IServiceTaxonomySearcher serviceTaxonomy, 
                IOptions<ServiceTaxonomySettings> settings,IOptions<CompositeSettings> compositeSettings, 
                ISessionService sessionService)  : base(dataProtectionProvider, compositeSettings, sessionService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _settings = settings.Value;
            Throw.IfNull(_settings.ApiUrl, nameof(_settings.ApiUrl));
            Throw.IfNull(_settings.ApiKey, nameof(_settings.ApiKey));
            Throw.IfNull(sessionService, nameof(sessionService));
            
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
            _apiUrl = _settings.ApiUrl;
            _apiKey = _settings.ApiKey;
            _sessionService = sessionService;

        }

        [HttpPost]
        [Route("MatchSkills/[controller]")]
        public  async Task<IActionResult> Body(string  enterJobInputAutocomplete)
        {
            ViewModel.Occupation = enterJobInputAutocomplete;
            var occupationId = await GetOccupationIdFromName(enterJobInputAutocomplete);

            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);

            var resultGet = await _sessionService.GetUserSession(primaryKeyFromCookie);

            if (resultGet.Occupations == null)
            {
                resultGet.Occupations = new HashSet<UsOccupation>();
            }
            resultGet.Occupations.Add(new UsOccupation(occupationId,enterJobInputAutocomplete,DateTime.Now));
            await _sessionService.UpdateUserSessionAsync(resultGet);

            var Skills = await _serviceTaxonomy.GetAllSkillsForOccupation<Skill[]>($"{_apiUrl}",
                _apiKey, occupationId);

            ViewModel.Skills = Skills.Where(s=>s.RelationshipType==RelationshipType.Essential).ToList(); 
            
            return base.Body();
        }
        [HttpPost]
        [Route("/MatchSkills/[controller]/AddSkills")]
        public async Task<IActionResult> AddSkills(IFormCollection formCollection)
        {
            var userSession = GetUserSession();

            foreach (var key in formCollection.Keys)
            { 
                string[] skill = key.Split("--");
                Throw.IfNull(skill[0], nameof(skill));
                Throw.IfNull(skill[1], nameof(skill));
                userSession.Skills.Add(new UsSkill(skill[0],skill[1],DateTime.Now));
            }
           
            await _sessionService.UpdateUserSessionAsync(userSession);
            
            return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.SkillsBasket}");
            
        }

        public async Task<string> GetOccupationIdFromName(string occupation)
        {
            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, bool.Parse(_settings.SearchOccupationInAltLabels));
            return occupations.SingleOrDefault(x => x.Name == occupation).Id;
        }
    }
}