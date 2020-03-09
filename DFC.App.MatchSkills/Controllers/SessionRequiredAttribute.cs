using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class SessionRequiredAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionService = (ISessionService)context.HttpContext.RequestServices.GetService(typeof(ISessionService));
            var session = await sessionService.GetUserSession();
            await next();
            ThrowNoSession(session);

            await base.OnActionExecutionAsync(context, next);
        }

        private void ThrowNoSession(object ob)
        {
            if (ob == null)
            {
                throw new Exception("No Session");
            }
        }
    }
}
