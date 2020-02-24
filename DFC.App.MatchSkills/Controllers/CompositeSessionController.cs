using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    public abstract class CompositeSessionController<TViewModel> : SessionController where TViewModel : CompositeViewModel, new()
    {
        protected TViewModel ViewModel { get; }
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, 
            ISessionService sessionService, ICookieService cookieService)
            : base(sessionService, cookieService)
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };
        }

        [HttpGet]
        [Route("/head/[controller]/{id?}")]
        public virtual IActionResult Head()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]/{id?}")]
        public virtual IActionResult BodyTop()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/breadcrumb/[controller]/{id?}")]
        public virtual IActionResult Breadcrumb()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]/{id?}")]
        public virtual Task<IActionResult> Body()
        {
            return Task.FromResult<IActionResult>(View(ViewModel));
        }

        protected virtual IActionResult RedirectWithError(string controller, string parameters = "")
        {
            

            if (!string.IsNullOrEmpty(parameters))
            {
                 parameters = $"&{parameters}";
            }

            return RedirectTo($"{controller}?errors=true{parameters}");
        }

        protected async Task<HttpResponseMessage> TrackPageInUserSession(UserSession session = null)
        {
            var primaryKeyFromCookie = TryGetPrimaryKey(this.Request);
            return await UpdateUserSession(primaryKeyFromCookie, ViewModel.Id.Value, session);
        }

        protected bool HasErrors()
        {
            var errorsString = Request.Query["errors"];
            var parsed = bool.TryParse(errorsString, out var error);
            return parsed && error;
        }

        protected IActionResult RedirectTo(string relativeAddress)
        {
            relativeAddress = $"~{ViewModel.CompositeSettings.Path}/" + relativeAddress;
            
            return Redirect(relativeAddress);
        }
    }
}