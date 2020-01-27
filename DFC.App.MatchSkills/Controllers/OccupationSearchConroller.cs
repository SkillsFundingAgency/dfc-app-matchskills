using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchController : BaseController
    {

        private const string PathName = "OccupationSearch";

        public OccupationSearchController(IDataProtectionProvider dataProtectionProvider) : base(dataProtectionProvider)
        {
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