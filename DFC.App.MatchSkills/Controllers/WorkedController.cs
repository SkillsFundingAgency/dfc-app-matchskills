using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels.Worked;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{
    public class WorkedController : CompositeSessionController<WorkedCompositeViewModel>
    {
        public WorkedController(IDataProtectionProvider dataProtectionProvider, IOptions<CompositeSettings> settings) 
            : base(dataProtectionProvider, settings)
        {
        }

        [HttpPost]
        [Route("/matchskills/[controller]")]
        public IActionResult Body(string choice)
        {
            if (!string.IsNullOrWhiteSpace(choice))
            {
                if (choice.Trim().ToLower() == "yes")
                {
                    return RedirectToAction("Body", "OccupationSearch");
                }
            }
            return View();
        }
    }
}
