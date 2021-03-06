﻿using DFC.App.MatchSkills.Application.Session.Interfaces;
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
        [Route("/head/{controller}")]
        [Route("/head")]
        public override IActionResult Head()
        { 
            return base.Head();
        }
        [Route("/bodytop/{controller}")]
        [Route("/bodytop")]
        public override IActionResult BodyTop()
        {
            return base.BodyTop();
        }
        [Route("/breadcrumb/{controller}")]
        [Route("/breadcrumb")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }

        [Route("/body/{controller}")]
        [Route("/body")]

        public async override Task<IActionResult> Body()
        {
            ViewModel.HasErrors = HasErrors();
            return RedirectTo(CompositeViewModel.PageId.OccupationSearch.Value);
        }

        [Route("/bodyfooter/{controller}")]
        [Route("/bodyfooter")]

        public override IActionResult BodyFooter()
        {
            return base.BodyFooter();
        }
        #endregion Default Routes
    }
}