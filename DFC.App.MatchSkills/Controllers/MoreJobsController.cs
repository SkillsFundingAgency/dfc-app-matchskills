﻿using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class MoreJobsController : CompositeSessionController<MoreJobsViewModel>
    {
        private readonly ServiceTaxonomySettings _settings;

        public MoreJobsController(IOptions<ServiceTaxonomySettings> settings,
            IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService )
            : base( compositeSettings,
                sessionService )
        {
            Throw.IfNull(settings, nameof(settings));
            _settings = settings.Value;
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var userSession = await GetUserSession();
            await TrackPageInUserSession(userSession);
            ViewModel.Occupations.LoadFromSession(userSession);
            ViewModel.SearchService = _settings.SearchService;
            return await base.Body();
        }
    }
}