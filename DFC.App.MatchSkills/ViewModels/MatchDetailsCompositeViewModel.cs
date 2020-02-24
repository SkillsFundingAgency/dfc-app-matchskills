using System.Collections.Generic;
using DFC.Personalisation.Domain.Models;
using NSubstitute.Routing.AutoValues;

namespace DFC.App.MatchSkills.ViewModels
{
    public class  MatchDetailsCompositeViewModel : CompositeViewModel
    {
        public MatchDetailsCompositeViewModel() : base(PageId.MatchDetails , "")
        {
        }

        public string CareerTitle { get; set; }
        public string CareerDescription { get; set; }
        public string[] MatchingSkills { get; set; }
        public string[] MissingSkills { get; set; }
    }
}
