using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchResultsController : BaseController
    {
        private const string PathName = "OccupationSearchResults";

        public OccupationSearchResultsController(IDataProtectionProvider dataProtectionProvider) : base()
        {
        }

        [HttpGet]
        [Route("/head/OccupationSearchResults")]
        public override IActionResult Head()
        {
            return View(ReturnPath("Head", "OccupationSearchResults"));
        }

        [HttpGet]
        [Route("/breadcrumb/"+ PathName)]
        public override IActionResult Breadcrumb()
        {
            return View(ReturnPath("Breadcrumb", "OccupationSearchResults"));
        }

        [HttpGet]
        [Route("/bodytop/"+ PathName)]
        public override IActionResult BodyTop()
        {
            return View(ReturnPath("bodytop"));
        }

        [HttpGet]
        [Route("/body/"+ PathName)]
        public override IActionResult Body()
        {
            return View(ReturnPath("body", "OccupationSearchResults"));
        }
    }
}