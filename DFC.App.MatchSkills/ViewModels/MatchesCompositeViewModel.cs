using DFC.App.MatchSkills.Models;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.ViewModels
{
    public class MatchesCompositeViewModel : CompositeViewModel
    {
        public ICollection<CareerMatch> CareerMatches { get; set; }

        public MatchesCompositeViewModel()
            : base(PageId.Matches, "Your career matches")
        {
            CareerMatches = new List<CareerMatch>();
        }
    }
}