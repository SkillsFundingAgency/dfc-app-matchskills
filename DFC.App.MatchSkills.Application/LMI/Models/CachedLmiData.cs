using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.MatchSkills.Application.LMI.Models
{
    public class CachedLmiData
    {
        public int SocCode { get; set; }
        public JobGrowth JobGrowth { get; set; }
        public DateTimeOffset LastChecked { get; set; }
    }
}
