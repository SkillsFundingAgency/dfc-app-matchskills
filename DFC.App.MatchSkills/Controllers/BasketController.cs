using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using DFC.Personalisation.Domain.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class BasketController : CompositeSessionController<SkillsBasketCompositeViewModel>
    {
        public BasketController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> compositeSettings)
            : base(dataProtectionProvider, compositeSettings)
        {
        }

        public override IActionResult Body()
        {
            // @ToDo: use real data
            ViewModel.Skills.Add(new Skill("skill1", "Dummy skill 1", SkillType.Competency));
            ViewModel.Skills.Add(new Skill("skill2", "Dummy skill 2", SkillType.Competency));
            ViewModel.Skills.Add(new Skill("skill3", "Dummy skill 3", SkillType.Competency));
            ViewModel.Skills.Add(new Skill("skill4", "Dummy skill 4", SkillType.Competency));

            return base.Body();
        }
    }
}
