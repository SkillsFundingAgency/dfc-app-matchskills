using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.WebUI.ViewModels;
using DFC.App.MatchSkills.WebUI.ViewComponents;
using DFC.Personalisation.Common.Net.RestClient;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;


namespace DFC.App.MatchSkills.WebUI.ViewComponents.SearchOccupationResults
{
    public class SearchOccupationResults : ViewComponent
    {
        private IServiceTaxonomySearcher _serviceTaxonomy;
        public SearchOccupationResults(IServiceTaxonomySearcher serviceTaxonomy)
        {
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
        }
        public async Task<IViewComponentResult> InvokeAsync(string occupation="furniture")
        {
            SearchOccupationResultsViewModel vm = new SearchOccupationResultsViewModel();
            var rc = new RestClient();
            
            var occupations =await _serviceTaxonomy.SearchOccupations<Occupation[]>("https://dev.api.nationalcareersservice.org.uk/GetAllOccupations/Execute/","8ed8640b25004e26992beb9164d95139",occupation);
            vm.Occupations = occupations;
            
        /*    vm.Occupations = new Occupation[]
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
