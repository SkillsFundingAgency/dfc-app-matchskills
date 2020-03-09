using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        public HomeController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService )
            : base(compositeSettings, sessionService)
        {
        }

        #region Default Routes

        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here

        [Route("/head")]
        public override IActionResult Head()
        {
            return base.Head();
        }

        [Route("/bodytop")]
        public override IActionResult BodyTop()
        {
            return base.BodyTop();
        }

        [Route("/breadcrumb")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }

        [Route("/body")]
        public override Task<IActionResult> Body()
        {
            return base.Body();
        }

        #endregion Default Routes
    }
}