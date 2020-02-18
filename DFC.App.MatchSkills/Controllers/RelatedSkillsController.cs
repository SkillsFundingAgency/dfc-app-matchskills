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
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Domain.Models;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.MatchSkills.Controllers
{
    public class RelatedSkillsController : CompositeSessionController<RelatedSkillsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
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
            _apiUrl = settings.Value.ApiUrl;
            _apiKey = settings.Value.ApiKey;

        }
        [HttpPost]
        [SessionRequired]
        [Route("/MatchSkills/[controller]/GetRelatedSkills")]
        public async Task<IActionResult> Body(string enterSkillsInputInput)
        {
            ViewModel.SearchTerm = enterSkillsInputInput;
            await TrackPageInUserSession();
            var userSession = await GetUserSession();

            //Skills in basket
            ViewModel.Skills.LoadFrom(userSession);

                        
            var skills = await _serviceTaxonomy.GetSkillsByLabel<Skill[]>($"{_apiUrl}",
                _apiKey, ViewModel.SearchTerm);
            List<Skill> filteredSkills = skills.Where(x => x.RelationshipType == RelationshipType.Essential).ToList();
            ViewModel.RelatedSkills.LoadFrom(filteredSkills);

            return await base.Body();

        }

   
    }
}