using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Application.LMI.Models
{
    public enum PredictionFilter
    {
        
        Qualification,
        Industry,
        Status,
        Region,
        Gender

    }
    public class WfPredictionRequest
    {
        [JsonProperty("soc")]
        public int SocCode { get; set; }
        [JsonProperty("filter")]
        public PredictionFilter FilterType { get; set; }

    }
}
