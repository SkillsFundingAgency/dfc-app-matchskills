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
    }
}
