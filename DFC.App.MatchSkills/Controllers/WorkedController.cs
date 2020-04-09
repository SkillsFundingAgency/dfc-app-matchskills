using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Controllers
{

    public class WorkedController : CompositeSessionController<WorkedCompositeViewModel>
    {
        private readonly IOptions<DysacSettings> _dysacSettings;
        private readonly IDysacSessionReader _dysacService;
        
        public WorkedController(IOptions<CompositeSettings> compositeSettings, ISessionService sessionService, IDysacSessionReader dysacService, 
            IOptions<DysacSettings> dysacSettings)
            : base(compositeSettings, sessionService )
        {
            Throw.IfNull(dysacSettings, nameof(dysacSettings));
            _dysacSettings = dysacSettings;
            _dysacService = dysacService;
        }

        public override async Task<IActionResult> Body()
        {
            var createSessionRequest = new CreateSessionRequest()
            {
                CurrentPage = CompositeViewModel.PageId.Worked.Value
            };
            await CreateUserSession(createSessionRequest);
            
            ViewModel.HasError = HasErrors();

            return await base.Body();
        }

        [HttpPost]
        [SessionRequired]
        public async Task<IActionResult> Body(WorkedBefore choice)
        {
            var userWorkedBefore = choice == WorkedBefore.Undefined ? (bool?)null : choice == WorkedBefore.Yes;
            var options = new TestModel();
            options.TestProperty = "string";
            switch (choice)
            {
                case WorkedBefore.Yes:
                    await UpdateUserSession(userWorkedBefore);
                    return RedirectTo(CompositeViewModel.PageId.Route.Value);
                
                case WorkedBefore.No:
                    await _dysacService.InitiateDysacOnly();
                    await UpdateUserSession(userWorkedBefore);
                    return Redirect(_dysacSettings.Value.DysacUrl); 
                    
                default:
                    return RedirectWithError(ViewModel.Id.Value);
            }
        }

        private async Task UpdateUserSession(bool? userWorkedBefore)
        {
            var session = await GetUserSession();
            session.UserHasWorkedBefore = userWorkedBefore;
            await UpdateUserSession(ViewModel.Id.Value, session);
        }
    }
}
