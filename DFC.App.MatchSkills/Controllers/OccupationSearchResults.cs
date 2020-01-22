using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchResultsController : BaseController
    {
        private const string PathName = "OccupationSearchResults";

        [HttpGet]
        [Route("/head/"+ PathName)]
        public override IActionResult Head()
        {
            return View(ReturnPath("Head", PathName));
        }

        [HttpGet]
        [Route("/breadcrumb/"+ PathName)]
        public override IActionResult Breadcrumb()
        {
            return View(ReturnPath("Breadcrumb", PathName));
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
            return View(ReturnPath("body", PathName));
        }

        [HttpGet]
        [Route("/sidebarright/"+ PathName)]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }
    }
}