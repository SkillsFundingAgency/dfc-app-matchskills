using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        public ErrorController(IOptions<CompositeSettings> compositeSettings, ISessionService sessionService)
            : base(compositeSettings, sessionService)
        {
        }

        public override async Task<IActionResult> Body()
        {            
            var userSession = await GetUserSession();
            if(null != userSession)
            {
                ViewModel.RTACode = userSession.UserSessionId;
            }

            return await base.Body();
        }
    }
}
