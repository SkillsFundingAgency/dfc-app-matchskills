using System;
using System.Linq;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;

namespace DFC.App.MatchSkills.Controllers
{
    public class BasketController : CompositeSessionController<SkillsBasketCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly int _minimumMatchingSkills;
        private readonly ISessionService _sessionService;
        private readonly string _skillUrlBase;

        public BasketController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService,
            IOptions<ServiceTaxonomySettings> settings,
            IServiceTaxonomySearcher serviceTaxonomy)
            : base( compositeSettings, sessionService, cookieService)
        {
            _serviceTaxonomy = serviceTaxonomy;
            _apiUrl = settings.Value.ApiUrl;
            _apiKey = settings.Value.ApiKey;
            _minimumMatchingSkills = settings.Value.MinimumMatchingSkills;
            _sessionService = sessionService;
            _skillUrlBase = $"{settings.Value.EscoUrl}/skill/";
        }

        public override async Task<IActionResult> Body()
        {
            await TrackPageInUserSession();
            var userSession = await GetUserSession();
            ViewModel.Skills.LoadFrom(userSession);

            return await base.Body();
        }

        [HttpPost]
        [Route("/MatchSkills/[controller]")]
        public async Task<IActionResult> Submit()
        {
            var userSession = await GetUserSession();

            if (userSession.Skills.Count > 0)
            {
                int minimumMatch = Math.Min(_minimumMatchingSkills, userSession.Skills.Count);
                var skillIds = userSession.Skills.Select(skill => skill.Id).ToArray();
                userSession.OccupationMatches = await _serviceTaxonomy.FindOccupationsForSkills(_apiUrl, _apiKey, skillIds, minimumMatch);
            }
            await _sessionService.UpdateUserSessionAsync(userSession);

            return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Matches}");
        }
    }
}
