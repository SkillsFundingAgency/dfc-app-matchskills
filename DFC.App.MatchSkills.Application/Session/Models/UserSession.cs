using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.Session.Models
{
    public class UserSession
    {
        [JsonIgnore]
        public string PrimaryKey => $"{PartitionKey}-{UserSessionId}";
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }
        [JsonProperty("id")]
        public string UserSessionId { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }
        public string CurrentPage { get; set; }
        public string PreviousPage { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public bool? UserHasWorkedBefore { get; set; }
        public bool? RouteIncludesDysac { get; set; }
        public List<UsOccupation> Occupations { get; set; }
        public List<UsSkill> Skills { get; set; }
        public string[] DysacJobCategories { get; set; }

    }
}
