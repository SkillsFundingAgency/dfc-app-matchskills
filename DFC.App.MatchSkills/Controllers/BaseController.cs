using Microsoft.AspNetCore.Mvc;


namespace DFC.App.MatchSkills.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController() { }
        

        public abstract IActionResult Head();
        public abstract IActionResult Breadcrumb();
        public abstract IActionResult BodyTop();
        public abstract IActionResult Body();

        protected string ReturnPath(string segmentName, string path = "")
        {
            string viewpath = $"/Views/{(string.IsNullOrWhiteSpace(path) ? "shared" : path)}/{segmentName}.cshtml";
            return viewpath;
        }
    }
}
