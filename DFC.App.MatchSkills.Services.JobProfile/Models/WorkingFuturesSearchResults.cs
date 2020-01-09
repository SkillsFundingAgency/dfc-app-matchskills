using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.MatchSkills.Services.JobProfile.Models
{
    public class WorkingFuturesSearchResults
    {
        public int Soc { get; set; }
        public List<PredictedEmploymentResults> PredictedEmployment { get; set; }
    }
}
