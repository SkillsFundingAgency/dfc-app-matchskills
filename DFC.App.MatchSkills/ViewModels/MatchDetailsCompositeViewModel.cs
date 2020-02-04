using System.Collections.Generic;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class  MatchDetailsCompositeViewModel : CompositeViewModel
    {
        public MatchDetailsCompositeViewModel() : base(PageId.MatchDetails , "")
        {
        }

        public string CareerTitle { get; set; }
        public string CareerDescription { get; set; }
        public List<Skill> MatchingSkills { get; set; }
        public List<Skill> MissingSkills { get; set; }
    }
}
