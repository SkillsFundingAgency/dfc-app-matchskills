using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class STOccupation
    {
        public string Occupation { get; set; }
        public string[] AlternativeLabels { get; set; }
        public DateTime LastModified { get; set; }
        public string Uri { get; set; }
    }
}
