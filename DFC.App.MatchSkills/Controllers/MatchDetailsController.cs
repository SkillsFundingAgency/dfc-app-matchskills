using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchDetailsController : CompositeSessionController<MatchDetailsCompositeViewModel>
    {
        private readonly IServiceTaxonomySearcher _serviceTaxonomy;
        private readonly ServiceTaxonomySettings _settings;
        private readonly ISessionService _sessionService;

        public MatchDetailsController(IServiceTaxonomySearcher serviceTaxonomy, 
            IOptions<ServiceTaxonomySettings> settings, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService, ICookieService cookieService) : base(compositeSettings, sessionService, cookieService)
        {
            Throw.IfNull(serviceTaxonomy, nameof(serviceTaxonomy));
            Throw.IfNull(settings, nameof(settings));
            _serviceTaxonomy = serviceTaxonomy ?? new ServiceTaxonomyRepository();
            _settings = settings.Value;
            _sessionService = sessionService;

        }

        [SessionRequired]
        [HttpGet]
        public override async Task<IActionResult> Body()
        {
            return await base.Body();
        }

        [SessionRequired]
        [HttpGet]
        [Route("/body/[controller]/{*id}")]
        public async Task<IActionResult> Body(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectPermanent(CompositeViewModel.PageId.Matches.Value);
            }

            ViewModel.MissingSkills = new List<Skill>()
                {
                    new Skill("1","to be thorough and pay attention to detail",SkillType.Competency),
                    new Skill("1","the ability to work well with others",SkillType.Competency),
                    new Skill("1","administration skills",SkillType.Competency),
                    new Skill("1","customer service skills",SkillType.Competency)
                };
            ViewModel.CareerDescription =
                "Accounting technicians handle day-to-day financial matters in all types of business.";
            ViewModel.MatchingSkills = new List<Skill>
                {
                    new Skill("1","the ability to use your initiative",SkillType.Competency),
                    new Skill("1","to be flexible and open to change",SkillType.Competency),
                    new Skill("1","maths knowledge",SkillType.Competency),
                    new Skill("1","excellent verbal communication skills",SkillType.Competency),
                    new Skill("1","to be able to use a computer and the main software packages confidently",SkillType.Competency)
                };
            ViewModel.CareerTitle = "Accounting technician";

            return await base.Body();
        }

    }
}
