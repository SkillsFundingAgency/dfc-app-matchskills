using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.ViewComponents.SkillsBasket
{
    public class SkillsBasket : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ICollection<Skill> skills)
        {
            return View("~/ViewComponents/SkillsBasket/Default.cshtml", skills);
        }
    }
}
