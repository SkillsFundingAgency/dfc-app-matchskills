using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DFC.App.MatchSkills.Application;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.WebUI.ViewModels;


namespace DFC.App.MatchSkills.WebUI.Controllers
{
    public class SearchOccupationController : Controller
    {
        private readonly ILogger<SearchOccupationController> _logger;
        private readonly IServiceTaxonomySearcher _stSearcher;

        public SearchOccupationController(
            ILogger<SearchOccupationController> logger,
            IServiceTaxonomySearcher stSearcher
        )
        {
           // Throw.IfNull(logger, nameof(logger));
           // Throw.IfNull(stSearcher, nameof(stSearcher));

            _logger = logger;
            _stSearcher = stSearcher;
        }
         
        [HttpPost]
        public IActionResult Search(string occupation)
        {
            OccupationSearchResultsViewModel vm = new OccupationSearchResultsViewModel();
            return View("","~/Views/OccupationSearch/SearchResults.cshtml",vm);
        }

    }
}