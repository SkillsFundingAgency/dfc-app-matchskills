using System;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class StOccupationSearchResult
    {

        public class OccupationSearchResult
        {
            public StsOccupation[] Occupations { get; set; }
        }

        public class StsOccupation
        {
            public string Description { get; set; }
            public string Occupation { get; set; }
            public DateTime LastModified { get; set; }
            public string[] AlternativeLabels { get; set; }
            public string Uri { get; set; }
            public Matches Matches { get; set; }
            public string Description { get; set; }
        }

        public class Matches
        {
            public string[] Occupation { get; set; }
            public object[] AlternativeLabels { get; set; }
        }

    }
}
