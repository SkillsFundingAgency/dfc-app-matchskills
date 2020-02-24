using System;
using System.Collections.Generic;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Models
{
    public class CareerMatch
    {
        public JobProfile JobProfile { get; set; }
        
        public string JobSectorGrowthDescription { get; set; }

        public ICollection<Skill> MatchedSkills { get; set; }

        public ICollection<Skill> UnMatchedSkills { get; set; }

        public int MatchingEssentialSkills { get; set; }


        public int MatchingOptionalSkills { get; set; }

        public int TotalOccupationEssentialSkills { get; set; }

        public int TotalOccupationOptionalSkills { get; set; }

        public int SourceSkillCount { get; set; }

        public string EscoURIGuid { get; set; }

        public CareerMatch()
        {
            JobProfile = new JobProfile();
            JobSectorGrowthDescription = string.Empty;
            MatchedSkills = new List<Skill>();
            UnMatchedSkills = new List<Skill>();
        }

        public int MatchStrengthPercentage
        {
            get
            {
                int matchStrength = 0;

                //   percentage match calculation = total number of skills matched in ST / total number of skills added to the skills list = % match  (eg. 8 skills matched in ST / 10 skills in skills list = 80% skills match)
                if (TotalOccupationEssentialSkills > 0)
                {
                    double matched = MatchingEssentialSkills;
                    double total = TotalOccupationEssentialSkills;

                    var pct = (matched / total) * 100;
                    matchStrength = Convert.ToInt32(Math.Round(pct, 0, MidpointRounding.AwayFromZero));
                }

                return matchStrength;
            }
        }
    }
}
