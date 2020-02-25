using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class SkillsGapRequest
    {
        [JsonProperty("skillList")]
        public string[] SkillList { get; set; }
        [JsonProperty("occupation")]
        public string Occupation { get; set; }
    }
}
