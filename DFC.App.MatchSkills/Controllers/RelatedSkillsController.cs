using System;
using System.Collections.Generic;
using System.Linq;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Domain.Models;
using Dfc.ProviderPortal.Packages;
using Microsoft.AspNetCore.Http;

namespace DFC.App.MatchSkills.Controllers
{
    public class RelatedSkillsController : CompositeSessionController<RelatedSkillsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ISessionService _sessionService;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public RelatedSkillsController(IServiceTaxonomySearcher serviceTaxonomy, 
            IOptions<ServiceTaxonomySettings> settings, IOptions<CompositeSettings> compositeSettings, ISessionService sessionService, ICookieService cookieService)
            : base(compositeSettings, sessionService, cookieService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            
            Throw.IfNull(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            Throw.IfNull(settings.Value.ApiKey, nameof(settings.Value.ApiKey));

            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _sessionService = sessionService;
            _apiUrl = settings.Value.ApiUrl;
            _apiKey = settings.Value.ApiKey;

        }

        [SessionRequired]
        [HttpGet]
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }

        [SessionRequired]
        [HttpGet]
        [Route("/body/[controller]/{*searchTerm}")]
        public async Task<IActionResult> Body(string searchTerm)
        {
            await GetRelatedSkills(searchTerm);
            return await base.Body();
        }

        [SessionRequired]
        [HttpPost]
        [Route("MatchSkills/[controller]/AddSkills")]
        public async Task<IActionResult> Body(IFormCollection formCollection, string searchTerm)
        {
            var search = searchTerm;
            if (formCollection.Keys.Count <= 1)
            {
                ViewModel.HasError = true;
                await GetRelatedSkills(search);
                return await base.Body();
            }
            var userSession = await GetUserSession();

            foreach (var key in formCollection.Keys)
            {
                if (key.Equals(nameof(searchTerm), StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                string[] skill = key.Split("--");
                Throw.IfNull(skill[0], nameof(skill));
                Throw.IfNull(skill[1], nameof(skill));
                userSession.Skills.Add(new UsSkill(skill[0], skill[1], DateTime.Now));
            }

            await _sessionService.UpdateUserSessionAsync(userSession);

            return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.SkillsBasket}");
        }

        public async Task GetRelatedSkills(string searchTerm)
        {
            ViewModel.SearchTerm = searchTerm;
            await TrackPageInUserSession();
            var userSession = await GetUserSession();

            ViewModel.Skills.LoadFrom(userSession);
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var skills = await _serviceTaxonomy.GetSkillsByLabel<Skill[]>($"{_apiUrl}",
                    _apiKey, ViewModel.SearchTerm);
                List<Skill> filteredSkills = skills.Where(x => x.RelationshipType == RelationshipType.Essential).ToList();
                ViewModel.RelatedSkills.LoadFrom(filteredSkills);
            }

        }

    }
}