using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{
    public class WorkedController : BaseController
    {
        private const string PathName = "Worked";

        public WorkedController(IDataProtectionProvider dataProtectionProvider) : base(dataProtectionProvider)
        {
        }


        [HttpGet]
        [Route("/head/"+ PathName)]
        public override IActionResult Head()
        {
            var vm = new HeadViewModel
            {
                PageTitle = PathName
            };
            return View(ReturnPath("Head"), vm);
        }

        [HttpGet]
        [Route("/breadcrumb/"+ PathName)]
        public override IActionResult Breadcrumb()
        {
            return View(ReturnPath("Breadcrumb"));
        }

        [HttpGet]
        [Route("/bodytop/"+ PathName)]
        public override IActionResult BodyTop()
        {
            return View(ReturnPath("BodyTopWithOutBanner", "Shared"));
        }

        [HttpGet]
        [Route("/body/"+ PathName)]
        public override IActionResult Body()
        {
            var sessionId = TryGetSessionId(Request);
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                AppendCookie(sessionId);
            }
            return View(ReturnPath("body", PathName));
        }


        [HttpPost]
        [Route("MatchSkills/body/" + PathName)]
        public IActionResult Body(string choice)
        {
            return View(ReturnPath("body", PathName));
        }


        [HttpGet]
        [Route("/sidebarright/" + PathName)]
        public override IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }


    }
}
