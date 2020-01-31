using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.ViewComponents.SearchOccupationResults
{
    public class SearchOccupationResults : ViewComponent
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;

        public SearchOccupationResults(IServiceTaxonomySearcher serviceTaxonomy,IOptions<ServiceTaxonomySettings>  settings)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }


        public async Task<IViewComponentResult> InvokeAsync(string occupation=default, bool altLabels=false)
        {
            var vm = new SearchOccupationResultsViewModel();
            
            var occupations =await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",_settings.ApiKey,occupation,altLabels);
            
            vm.Occupations = occupations;
            
            
            vm.Title = occupation;
            return View("~/ViewComponents/SearchOccupationResults/Default.cshtml",vm);
        }
    }
}
