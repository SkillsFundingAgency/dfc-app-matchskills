using DFC.App.MatchSkills.Application.Session.Interfaces;
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
        private readonly ISessionService _sessionService;
        protected CompositeSessionController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings, ISessionService sessionService)
            : base(dataProtectionProvider,sessionService)
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("/head/[controller]")]
        public virtual IActionResult Head()
        {
            var primaryKey = TryGetPrimaryKey(this.Request);
            if (string.IsNullOrWhiteSpace(primaryKey))
            {
                AppendCookie(_sessionService.GeneratePrimaryKey());
            }
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