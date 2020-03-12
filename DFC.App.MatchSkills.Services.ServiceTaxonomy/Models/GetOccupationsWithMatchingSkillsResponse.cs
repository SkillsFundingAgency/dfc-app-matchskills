using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class GetOccupationsWithMatchingSkillsResponse
    {
        public class MatchedOccupation
        {
            [JsonProperty("uri")]
            public string Uri { get; set; }

            [JsonProperty("jobProfileUri")]
            public string JobProfileUri { get; set; }

            [JsonProperty("jobProfileTitle")]
            public string JobProfileTitle { get; set; }

            [JsonProperty("jobProfileDescription")]
            public string JobProfileDescription { get; set; }

            [JsonProperty("totalOccupationEssentialSkills")]
            public int TotalOccupationEssentialSkills { get; set; }
            
            [JsonProperty("totalOccupationOptionalSkills")]
            public int TotalOccupationOptionalSkills{ get; set; }
            
            [JsonProperty("matchingEssentialSkills")]
            public int MatchingEssentialSkills { get; set; }
            
            [JsonProperty("matchingOptionalSkills")]
            public int MatchingOptionalSkills { get; set; }
            
            [JsonProperty("lastModified")]
            public DateTime LastModified { get; set; }
            [JsonProperty("socCode")]
            public int SocCode { get; set; }
        }

        [JsonProperty("matchingOccupations")]
        public IList<MatchedOccupation> MatchingOccupations { get; set; }

        public GetOccupationsWithMatchingSkillsResponse()
        {
            MatchingOccupations = new List<MatchedOccupation>();
        }
        
    }
}
