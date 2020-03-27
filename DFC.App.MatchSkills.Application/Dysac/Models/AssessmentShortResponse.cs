using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.MatchSkills.Application.Dysac.Models
{
    public class AssessmentShortResponse
    {
        public string PartitionKey { get; set; }
        public string SessionId { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

