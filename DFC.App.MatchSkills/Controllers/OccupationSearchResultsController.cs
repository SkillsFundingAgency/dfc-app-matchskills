using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchResultsController : SessionController
    {
        private const string PathName = "OccupationSearchResults";

        public OccupationSearchResultsController(IDataProtectionProvider dataProtectionProvider) : base(dataProtectionProvider)
        {
        }

        [HttpGet]
        [Route("/head/OccupationSearchResults")]
        public IActionResult Head()
        {
            return View();
        }

        [HttpGet]
        [Route("/breadcrumb/"+ PathName)]
        public IActionResult Breadcrumb()
        {
            return View();
        }

        [HttpGet]
        [Route("/bodytop/"+ PathName)]
        public IActionResult BodyTop()
        {
            return View();
        }

        [HttpGet]
        [Route("/body/"+ PathName)]
        public IActionResult Body()
        {
            return View();
        }

        [HttpGet]
        [Route("/sidebarright/"+ PathName)]
        public IActionResult SidebarRight()
        {
            return View();
        }
    }
}