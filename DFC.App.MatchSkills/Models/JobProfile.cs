using System.Collections.Generic;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Models
{
    // Probably needs refactoring in to common
    public class JobProfile
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public ICollection<Skill> Skills { get; set; }

        public JobProfile()
        {
            Title = string.Empty;
            Description = string.Empty;
            Url = string.Empty;
            Skills = new List<Skill>();
        }
    }
}
