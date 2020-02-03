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

namespace DFC.App.MatchSkills.Controllers
{
    public class SelectSkillsController :  CompositeSessionController<SelectSkillsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        
        public SelectSkillsController(IDataProtectionProvider dataProtectionProvider,IServiceTaxonomySearcher serviceTaxonomy, IOptions<ServiceTaxonomySettings> settings,IOptions<CompositeSettings> compositeSettings)  : base(dataProtectionProvider, compositeSettings)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }

        [HttpPost]
        [Route("MatchSkills/body/[controller]")]
        public   async Task<IActionResult> Body(string  enterJobInputAutocomplete)
        {
            ViewModel.Occupation = enterJobInputAutocomplete;

            var occupationId = await GetOccupationIdFromName(enterJobInputAutocomplete);
            
            ViewModel.Skills = await _serviceTaxonomy.GetAllSkillsForOccupation<Skill[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupationId);
            
            return base.Body();
        }

        public async Task<string> GetOccupationIdFromName(string occupation)
        {
            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, bool.Parse(_settings.SearchOccupationInAltLabels));
            return occupations.Single(x => x.Name == occupation).Id;
        }
    }
}