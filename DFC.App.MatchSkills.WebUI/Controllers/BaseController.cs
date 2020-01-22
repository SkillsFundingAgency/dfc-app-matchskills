using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        public abstract IActionResult Head();
        public abstract IActionResult Breadcrumb();
        public abstract IActionResult BodyTop();
        public abstract IActionResult Body();
        public abstract IActionResult SidebarRight();



        protected string ReturnPath(string segmentName, string path = "")
        {
            return $"/Views/{(string.IsNullOrWhiteSpace(path) ? "home" : path)}/{segmentName}.cshtml";
        }
    }
}
