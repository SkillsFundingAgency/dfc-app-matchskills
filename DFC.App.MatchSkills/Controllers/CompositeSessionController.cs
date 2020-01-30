using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    public abstract class CompositeSessionController<TViewModel> : SessionController where TViewModel : CompositeViewModel, new()
    {
        protected TViewModel ViewModel { get; }

        protected CompositeSessionController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings)
            : base(dataProtectionProvider)
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };
        }

        [HttpGet]
        [Route("/head/[controller]")]
        public virtual IActionResult Head()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]")]
        public virtual IActionResult BodyTop()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/breadcrumb/[controller]")]
        public virtual IActionResult Breadcrumb()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]")]
        public virtual IActionResult Body()
        {
            var sessionId = TryGetSessionId(Request);
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                AppendCookie(sessionId);
            }

            return View(ViewModel);
        }

        [HttpGet]
        [Route("/sidebarright/[controller]")]
        public virtual IActionResult SidebarRight()
        {
            return View(ViewModel);
        }
    }
}