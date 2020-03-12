using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Interfaces;
using DFC.App.MatchSkills.Models;
using DFC.App.MatchSkills.ViewModels;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Controllers
{

    public class OccupationSearchResultsController : CompositeSessionController<OccupationSearchResultsCompositeViewModel>
    {
        
        public OccupationSearchResultsController(IOptions<CompositeSettings> compositeSettings,
            ISessionService sessionService ) 
            : base(compositeSettings,
                sessionService )
        {
            
        }

       
    }
}