using DFC.App.MatchSkills.Application.LMI.Interfaces;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly ILmiService _lmiService;

        public BasketController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService,
            IOptions<ServiceTaxonomySettings> settings,
            IServiceTaxonomySearcher serviceTaxonomy,
            ILmiService lmiService)
            : base( compositeSettings, sessionService, cookieService)
        {
            _serviceTaxonomy = serviceTaxonomy;
            _apiUrl = settings.Value.ApiUrl;
            _apiKey = settings.Value.ApiKey;
            _minimumMatchingSkills = settings.Value.MinimumMatchingSkills;
            _sessionService = sessionService;
            _skillUrlBase = $"{settings.Value.EscoUrl}/skill/";
            _lmiService = lmiService;
        }

        public override async Task<IActionResult> Body()
        {
            await TrackPageInUserSession();
            var userSession = await GetUserSession();
            ViewModel.Skills.LoadFrom(userSession);

            return await base.Body();
        }

        [HttpPost]
        [DynamicRoute("{apppath}/[controller]")]
        public async Task<IActionResult> Submit()
        {
            var userSession = await GetUserSession();

            if (userSession.Skills.Count > 0)
            {
                int minimumMatch = Math.Min(_minimumMatchingSkills, userSession.Skills.Count);
                var skillIds = userSession.Skills.Select(skill => skill.Id).ToArray();
                userSession.OccupationMatches = await _serviceTaxonomy.FindOccupationsForSkills(_apiUrl, _apiKey, skillIds, minimumMatch);
                userSession.OccupationMatches =
                    _lmiService.GetPredictionsForGetOccupationMatches(userSession.OccupationMatches);
            }
            await _sessionService.UpdateUserSessionAsync(userSession);

            return RedirectPermanent($"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.Matches}");
        }
    }
}
