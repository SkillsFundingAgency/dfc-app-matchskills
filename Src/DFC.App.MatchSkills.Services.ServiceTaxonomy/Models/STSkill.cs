using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class STSkill
    {
        public string SkillType { get; set; }
        
        public string Skill { get; set; }
        
        public string[] AlternativeLabels { get; set; }
        
        public string Uri { get; set; }

    }
}
