using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        [HttpGet]
        [Route("/head/")]
        public virtual IActionResult Head()
        {
            return View(ReturnPath("Head"));
        }

        [HttpGet]
        [Route("/breadcrumb/")]
        public virtual IActionResult Breadcrumb()
        {
            return View(ReturnPath("Breadcrumb"));
        }

        [HttpGet]
        [Route("/bodytop/")]
        public virtual IActionResult BodyTop()
        {
            return View(ReturnPath("bodytop"));
        }

        [HttpGet]
        [Route("/sidebarright/")]
        public virtual IActionResult SidebarRight()
        {
            return View(ReturnPath("sidebarright"));
        }

        [HttpGet]
        [Route("/body/")]
        public abstract IActionResult Body();

        protected string ReturnPath(string segmentName, string path = "")
        {
            return $"/Views/{(string.IsNullOrWhiteSpace(path) ? "home" : path)}/{segmentName}.cshtml";
        }
    }
}
