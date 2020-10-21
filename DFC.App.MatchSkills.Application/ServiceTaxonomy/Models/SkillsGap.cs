using DFC.App.MatchSkills.Application.LMI.Models;

namespace DFC.App.MatchSkills.Application.ServiceTaxonomy.Models
{
    public class SkillsGap
    {
        public string CareerTitle { get; set; }

        public string[] MissingSkills { get; set; }
        public string[] OptionalMissingSkills { get; set; }

        public string CareerDescription { get; set; }

        public string[] MatchingSkills { get; set; }
        public string[] OptionalMatchingSkills { get; set; }
        public JobGrowth JobGrowth { get; set; }

    }
}

