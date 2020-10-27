using System.Collections.Generic;
using System.Linq;
using DFC.App.MatchSkills.Application.LMI.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class  MatchDetailsCompositeViewModel : CompositeViewModel
    {
        public MatchDetailsCompositeViewModel() : base(PageId.MatchDetails , "")
        {
        }

        public string CareerTitle { get; set; }
        public string CareerDescription { get; set; }
        public IOrderedEnumerable<KeyValuePair<string, bool>> MatchingSkills { get; set; }
        public IOrderedEnumerable<KeyValuePair<string, bool>> OptionalMatchingSkills { get; set; }
        public JobGrowth JobGrowth { get; set; }
    }
}
