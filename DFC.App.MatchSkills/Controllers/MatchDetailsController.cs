using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Controllers
{
    public class MatchDetailsController : CompositeSessionController<MatchDetailsCompositeViewModel>
    {
        public MatchDetailsController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService) : base(dataProtectionProvider, compositeSettings, sessionService)
        {
        }

        public override Task<IActionResult> Body()
        {
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

            return base.Body();
        }
    }
}
