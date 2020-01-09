using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.Services.JobProfile.Models
{
    public class SocSearchResults
    {
        public int Soc { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Qualifications { get; set; }
        public string Tasks { get; set; }
        [JsonProperty("Add_titles")]
        public List<string> AddTitles { get; set; }
    }
}
