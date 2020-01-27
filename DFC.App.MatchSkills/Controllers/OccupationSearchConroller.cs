using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : BaseController
    {
        private const string PathName = "OccupationSearch";

        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;

       
        public OccupationSearchController(IDataProtectionProvider dataProtectionProvider,IServiceTaxonomySearcher serviceTaxonomy,
            IOptions<ServiceTaxonomySettings> settings) 
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
        }

        [HttpGet,HttpPost]
        [Route("/OccupationSearch")]
        public async Task<IEnumerable> Search(string occupation)
        {
            var occupations = await _serviceTaxonomy.SearchOccupations<Occupation[]>($"{_settings.ApiUrl}",
                _settings.ApiKey, occupation, bool.Parse(_settings.SearchOccupationInAltLabels));

            return occupations.Select(x =>x.Name).ToList();
        }

        [HttpPost,ValidateAntiForgeryToken]
        [Route("/OccupationSearch/GetOccupationSkills")]
        public async Task<IEnumerable> GetOccupationSkills(IFormCollection collection)
        {
            var occupation = collection["input-autocomplete"];
           
            return null;
        }

        
        #region OccupationSearchCUI

        [HttpGet]
        [Route("/Index/OccupationSearch")]
        public IActionResult Index()
        {
            var vm = new OccupationSearchViewModel()
            {
                SearchService = _settings.SearchService
            };
            return View(ReturnPath("Index", "OccupationSearch"),vm);
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
            var vm = new OccupationSearchViewModel()
            {
                SearchService = _settings.SearchService
            };
            return View(ReturnPath("body", "OccupationSearch"),vm);
        }

        [HttpGet]
        [Route("/sidebarright/" + PathName)]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }
        #endregion
    }

}