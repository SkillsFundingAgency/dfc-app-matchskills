using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class SelectSkillsController :  CompositeSessionController<SelectSkillsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly string _cdn;
        private readonly ISessionService _sessionService;

        public SelectSkillsController(IServiceTaxonomySearcher serviceTaxonomy, 
                IOptions<ServiceTaxonomySettings> settings,IOptions<CompositeSettings> compositeSettings, 
                ISessionService sessionService, ICookieService cookieService)  : base(compositeSettings, sessionService, cookieService)
        {
            
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            
            Throw.IfNull(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            Throw.IfNull(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNull(sessionService, nameof(sessionService));
         
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _apiUrl = settings.Value.ApiUrl;
            _apiKey = settings.Value.ApiKey;
            _sessionService = sessionService;
            _cdn = compositeSettings.Value.CDN ?? "";

        }

        private async Task GetSessionData()
        {
            var resultGet = await GetUserSession();
            await TrackPageInUserSession(resultGet);

            Throw.IfNull(resultGet.Occupations, nameof(resultGet.Occupations));
            
            var occupation = resultGet.Occupations.OrderByDescending(o => o.DateAdded).First();
            
            ViewModel.Occupation = occupation.Name;
            
            var Skills = await _serviceTaxonomy.GetAllSkillsForOccupation<Skill[]>($"{_apiUrl}",
                _apiKey, occupation.Id);

            ViewModel.Skills = Skills.Where(s=>s.RelationshipType==RelationshipType.Essential).ToList();
            ViewModel.CDN = _cdn;
        }
         public override async Task<IActionResult> Body()
         {
             await GetSessionData();
             ViewModel.HasError = HasErrors();
             
             return await base.Body();
        }
        
        [HttpPost]
        [Route("/MatchSkills/[controller]")]
        public async Task<IActionResult> Body(IFormCollection formCollection)
        {
            await GetSessionData();
            if (formCollection.Keys.Count == 0)
            {
                ViewModel.HasError = true;
                return RedirectWithError(ViewModel.Id.Value);
            }
            var userSession = await GetUserSession();

            foreach (var key in formCollection.Keys)
            { 
                string[] skill = key.Split("--");
                Throw.IfNull(skill[0], nameof(skill));
                Throw.IfNull(skill[1], nameof(skill));
                userSession.Skills.Add(new UsSkill(skill[0],skill[1]));
            }
           
            await _sessionService.UpdateUserSessionAsync(userSession);

            return RedirectTo(CompositeViewModel.PageId.SkillsBasket.Value);
        }

        
        [Route("/body/[controller]/SkillSelectToggle")]
        [SessionRequired]
        public async Task<IActionResult> SkillSelectToggle(bool toggle)
        {
            await GetSessionData();
            ViewModel.AllSkillsSelected = !toggle;
            return View("body",ViewModel);
        }
       
    }
}