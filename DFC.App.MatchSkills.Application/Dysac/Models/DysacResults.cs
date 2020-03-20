using System.Collections.Generic;

namespace DFC.App.MatchSkills.Application.Dysac.Models
{
    public class DysacResults
    {
        public string SessionId { get; set; }
        public List<JobCategory> JobCategories { get; set; }
        public List<string> Traits { get; set; }
        public long JobFamilyCount { get; set; }
        public long JobFamilyMoreCount { get; set; }
        public string AssessmentType { get; set; }
        public List<string> JobProfiles { get; set; }
        public List<string> WhatYouToldUs { get; set; }
    }

    public class JobCategory
    {
        public string JobFamilyCode { get; set; }
        public string JobFamilyName { get; set; }
        public string JobFamilyText { get; set; }
        public string JobFamilyUrl { get; set; }
        public long TraitsTotal { get; set; }
        public long Total { get; set; }
        public double NormalizedTotal { get; set; }
        public List<TraitValue> TraitValues { get; set; }
        public object FilterAssessment { get; set; }
        public long TotalQuestions { get; set; }
        public bool ResultsShown { get; set; }
    }

    public class TraitValue
    {
        public string TraitCode { get; set; }
        public long Total { get; set; }
        public double NormalizedTotal { get; set; }
    }

}
