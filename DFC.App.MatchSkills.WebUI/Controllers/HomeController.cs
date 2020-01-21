using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        [Route("/body/home")]
        public override IActionResult Body()
        {
            return View(ReturnPath("body"));
        }
    }
}