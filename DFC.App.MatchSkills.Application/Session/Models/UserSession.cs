using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DFC.App.MatchSkills.Application.Dysac.Models;

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

        public DateTime SessionCreatedDate { get; set; }
        public string CurrentPage { get; set; }
        public string PreviousPage { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public bool? UserHasWorkedBefore { get; set; }
        public bool? RouteIncludesDysac { get; set; }
        public ISet<UsOccupation> Occupations { get; set; }
        public ISet<UsSkill> Skills { get; set; }
        public DysacJobCategory[] DysacJobCategories { get; set; }
        public IList<OccupationMatch> OccupationMatches { get; set; }
        public SortBy MatchesSortBy { get; set; } = SortBy.MatchPercentage;
        public SortDirection MatchesSortDirection { get; set; } = SortDirection.Descending;
        public ISet<UsSkill> SkillsToRemove { get; set; }
        public bool? DysacCompleted { get; set; }
        public UserSession()
        {
            Occupations = new HashSet<UsOccupation>();
            Skills = new HashSet<UsSkill>();
            SkillsToRemove = new HashSet<UsSkill>();
            OccupationMatches = new List<OccupationMatch>();
        }
    }
}
