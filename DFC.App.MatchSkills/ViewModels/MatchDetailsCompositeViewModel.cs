﻿using DFC.App.MatchSkills.Application.LMI.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class  MatchDetailsCompositeViewModel : CompositeViewModel
    {
        public MatchDetailsCompositeViewModel() : base(PageId.MatchDetails , "")
        {
        }

        public string CareerTitle { get; set; }
        public string CareerDescription { get; set; }
        public string[] MatchingSkills { get; set; }
        public string[] MissingSkills { get; set; }
        public JobGrowth JobGrowth { get; set; }
    }
}
