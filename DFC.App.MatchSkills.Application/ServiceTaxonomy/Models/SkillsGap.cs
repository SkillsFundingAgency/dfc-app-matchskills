using DFC.App.MatchSkills.Application.LMI.Models;

namespace DFC.App.MatchSkills.Application.ServiceTaxonomy.Models
{
    public class SkillsGap
    {
        
        public string CareerTitle { get; set; }

        public string[] MissingSkills { get; set; } = new string[0];
        public string[] OptionalMissingSkills { get; set; } = new string[0];

        public string CareerDescription { get; set; }

        public string[] MatchingSkills { get; set; } = new string[0];
        public string[] OptionalMatchingSkills { get; set; } = new string[0];
        public JobGrowth JobGrowth { get; set; }

    }
}

