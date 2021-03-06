﻿using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.Personalisation.Domain.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace DFC.App.MatchSkills.Models
{
    public class CareerMatch
    {
        public JobProfile JobProfile { get; set; }
        
        public JobGrowth JobSectorGrowthDescription { get; set; }

        public ICollection<Skill> MatchedSkills { get; set; }

        public ICollection<Skill> UnMatchedSkills { get; set; }

        public int MatchingEssentialSkills { get; set; }


        public int MatchingOptionalSkills { get; set; }

        public int TotalOccupationEssentialSkills { get; set; }

        public int TotalOccupationOptionalSkills { get; set; }

        public int SourceSkillCount { get; set; }

        private readonly IOptions<CompositeSettings> _compositeSettings;
        public CareerMatch(IOptions<CompositeSettings> compositeSettings)
        {
            JobProfile = new JobProfile();
            MatchedSkills = new List<Skill>();
            UnMatchedSkills = new List<Skill>();
            _compositeSettings = compositeSettings;
        }

        public int MatchStrengthPercentage { get; set; }

        public string GetDetailsUrl(string jobProfileUrl)
        {
            Throw.IfNullOrEmpty(jobProfileUrl, nameof(jobProfileUrl));
            
            string url = $"{_compositeSettings.Value.Path}/MatchDetails?id={jobProfileUrl}";

            return url;
        }
    }
}
