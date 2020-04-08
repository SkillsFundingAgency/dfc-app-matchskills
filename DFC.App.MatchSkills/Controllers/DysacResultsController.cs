using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class DysacResultsController : CompositeSessionController<DysacResultsCompositeViewModel>
    {
        public DysacResultsController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService)
            : base(compositeSettings, sessionService)
        {
        }

        [SessionRequired]
        public override async Task<IActionResult> Body()
        {
            var session = await GetUserSession();
            session.DysacCompleted = true;
            await UpdateUserSession(CompositeViewModel.PageId.OccupationSearch.Value, session);
            return RedirectTo(CompositeViewModel.PageId.OccupationSearch.Value);
        }
    }
}