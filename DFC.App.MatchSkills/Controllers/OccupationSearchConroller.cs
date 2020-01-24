using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Dfc.ProviderPortal.Packages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : BaseController
    {
        private const string PathName = "OccupationSearch";

        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        public OccupationSearchController(IServiceTaxonomySearcher serviceTaxonomy,IOptions<ServiceTaxonomySettings>  settings)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }
        

        [Route("/OccupationSearch")]
        public async Task<IActionResult> Search(string occupation)  
        {
            var _serviceTaxonomy = new ServiceTaxonomyRepository();

            var vm = new SearchOccupationResultsViewModel();

            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, Boolean.Parse(_settings.SearchOccupationInAltLabels));
            
            return   Json (occupations.Select(x=>x.Name).ToArray());

        }  

        [HttpGet]
        [Route("/head/OccupationSearch")]
        public override IActionResult Head()
        {
            return View(ReturnPath("Head", "OccupationSearch"));
        }

        [HttpGet]
        [Route("/breadcrumb/OccupationSearch")]
        public override IActionResult Breadcrumb()
        {
            return View(ReturnPath("Breadcrumb", "OccupationSearch"));
        }

        [HttpGet]
        [Route("/bodytop/OccupationSearch")]
        public override IActionResult BodyTop()
        {
            return View(ReturnPath("bodytop"));
        }

        [HttpGet]
        [Route("/body/OccupationSearch")]
        public override IActionResult Body()
        {
            return View(ReturnPath("body", "OccupationSearch"));
        }

        [HttpGet]
        [Route("/sidebarright/"+ PathName)]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }
    }
}