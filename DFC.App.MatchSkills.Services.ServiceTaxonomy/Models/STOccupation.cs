using System;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class StOccupation
    {
        public string Occupation { get; set; }
        public string[] AlternativeLabels { get; set; }
        public DateTime LastModified { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
    }
}
