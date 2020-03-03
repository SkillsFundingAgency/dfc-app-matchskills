using System;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.ServiceTaxonomy.Models
{
    public class OccupationMatch
    {
        public string JobProfileTitle { get; set; }

        public string JobProfileDescription { get; set; }
        public string JobProfileUri { get; set; }

        public DateTime LastModified { get; set; }

        public int MatchingEssentialSkills { get; set; }


        public int MatchingOptionalSkills { get; set; }

        public int TotalOccupationEssentialSkills { get; set; }

        public int TotalOccupationOptionalSkills { get; set; }

        public string Uri { get; set; }

        [JsonIgnore]
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
