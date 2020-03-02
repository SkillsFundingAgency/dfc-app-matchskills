using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.Personalisation.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace DFC.App.MatchSkills.Models
{
    public class CareerMatch
    {
        public JobProfile JobProfile { get; set; }
        
        public JobGrowth JobSectorGrowthDescription { get; set; }

        public ICollection<Skill> MatchedSkills { get; set; }

        public ICollection<Skill> UnMatchedSkills { get; set; }

        public int MatchingEssentialSkills { get; set; }


        public int MatchingOptionalSkills { get; set; }

        public int TotalOccupationEssentialSkills { get; set; }

        public int TotalOccupationOptionalSkills { get; set; }

        public int SourceSkillCount { get; set; }

        public CareerMatch()
        {
            JobProfile = new JobProfile();
            MatchedSkills = new List<Skill>();
            UnMatchedSkills = new List<Skill>();
        }

        public int MatchStrengthPercentage { get; set; }

        public string GetDetailsUrl(string jobProfileUrl)
        {
            Throw.IfNullOrEmpty(jobProfileUrl, nameof(jobProfileUrl));
            var jobProfileGuid = jobProfileUrl.Split('/').Last();
            string url = $"/matchskills/MatchDetails?id={jobProfileGuid}";

            return url;
        }
    }
}
