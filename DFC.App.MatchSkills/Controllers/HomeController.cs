using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{
    public class HomeController : BaseController
    {
        private const string PageTitle = "Discover your skills and careers";

        public HomeController(IDataProtectionProvider dataProtectionProvider) : base(dataProtectionProvider)
        {
        }

        [HttpGet]
        [Route("/head/")]
        public override IActionResult Head()
        {
            var vm = new HeadViewModel
            {
                PageTitle = PageTitle
            };
            return View(ReturnPath("Head"), vm);
        }

        [HttpGet]
        [Route("/breadcrumb/")]
        public override IActionResult Breadcrumb()
        {
            return View(ReturnPath("BreadCrumb"));
        }

        [HttpGet]
        [Route("/bodytop/")]
        public override IActionResult BodyTop()
        {
            return View(ReturnPath("BodyTop"));
        }

        [HttpGet]
        [Route("/body/")]
        public override IActionResult Body()
        {
            return View(ReturnPath("Body"));
        }

        [HttpGet]
        [Route("/sidebarright/")]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("SidebarRight"));
        }



    }
}