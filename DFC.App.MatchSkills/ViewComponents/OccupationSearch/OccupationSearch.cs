using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.ViewComponents.OccupationSearch
{
    public class OccupationSearch : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(OccupationSearchComponentModel model)
        {
            model ??= new OccupationSearchComponentModel();

            return View("~/ViewComponents/OccupationSearch/Default.cshtml", model);
        }
    }
}
