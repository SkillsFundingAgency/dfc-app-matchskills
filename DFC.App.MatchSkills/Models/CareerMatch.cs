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

                if (MatchedSkills.Count > 0 && UnMatchedSkills.Count > 0)
                {
                    decimal total = MatchedSkills.Count + UnMatchedSkills.Count;
                    matchStrength = Convert.ToInt32(Math.Round((MatchedSkills.Count / total) * 100, 0, MidpointRounding.AwayFromZero));
                }

                return matchStrength;
            }
        }
    }
}
