using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{
    public class SessionRequiredAttribute : ActionFilterAttribute, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sessionService = (ISessionService)context.HttpContext.RequestServices.GetService(typeof(ISessionService));
            var cookieService = (ICookieService) context.HttpContext.RequestServices.GetService(typeof(ICookieService));
            var cookie =
                cookieService.TryGetPrimaryKey(context.HttpContext.Request, context.HttpContext.Response);
            ThrowNoSession(cookie);
            var session = await sessionService.GetUserSession(cookie);
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
