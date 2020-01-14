using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.WebUI.ViewModels;
using DFC.App.MatchSkills.WebUI.ViewComponents;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using Dfc.ProviderPortal.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace DFC.App.MatchSkills.WebUI.ViewComponents.SearchOccupationResults
{
    public class SearchOccupationResults : ViewComponent
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private ServiceTaxonomySettings _settings;

        public SearchOccupationResults(IServiceTaxonomySearcher serviceTaxonomy,IOptions<ServiceTaxonomySettings>  settings)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }


        public async Task<IViewComponentResult> InvokeAsync(string occupation=default, bool altLabels=false)
        {
            SearchOccupationResultsViewModel vm = new SearchOccupationResultsViewModel();
            
            var occupations =await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",_settings.ApiKey,occupation,altLabels);
            
            vm.Occupations = occupations;
            
            /*vm.Occupations = new Occupation[]
            {
                new Occupation("1","Furniture 1",DateTime.Now), 
                new Occupation("2","Furniture 2",DateTime.Now), 
                new Occupation("3","Furniture 3",DateTime.Now) ,
                new Occupation("4","Furniture 4",DateTime.Now) ,
                new Occupation("5","Furniture 5",DateTime.Now), 
                new Occupation("6","Furniture 6",DateTime.Now), 
                new Occupation("7","Furniture 7",DateTime.Now) ,
                new Occupation("8","Furniture 8",DateTime.Now) ,
                new Occupation("9","Furniture 9",DateTime.Now), 
                new Occupation("10","Furniture 10",DateTime.Now), 
                new Occupation("11","Furniture 11",DateTime.Now) ,
                new Occupation("12","Furniture 12",DateTime.Now) 
            };
            */
            vm.Title = occupation;
            return View("~/ViewComponents/SearchOccupationResults/Default.cshtml",vm);
        }
    }
}
