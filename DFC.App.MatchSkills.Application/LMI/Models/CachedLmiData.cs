using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.LMI.Models
{
    public class CachedLmiData
    {
        [JsonProperty("id")]
        public string SocCode { get; set; }
        public JobGrowth JobGrowth { get; set; }
        public DateTimeOffset DateWritten { get; set; }
    }
}
