using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{
    public class HomeController : BaseController
    {
        private const string PageTitle = "Discover your skills and careers";

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
            return View(ReturnPath("Breadcrumb"));
        }

        [HttpGet]
        [Route("/bodytop/")]
        public override IActionResult BodyTop()
        {
            return View(ReturnPath("bodytop"));
        }

        [HttpGet]
        [Route("/body/")]
        public override IActionResult Body()
        {
            return View(ReturnPath("body"));
        }

        [HttpGet]
        [Route("/sidebarright/")]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }


    }
}