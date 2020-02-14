using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.ViewComponents.SkillsList
{
    public class SkillsListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(SkillsListViewModel model)
        {
            return View("~/ViewComponents/SkillsList/Default.cshtml", model);
        }

    }
}
