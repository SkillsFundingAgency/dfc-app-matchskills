using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class RelatedSkillsCompositeViewModel : CompositeViewModel
    {
        public RelatedSkillsCompositeViewModel() : base(PageId.RelatedSkills, "Select Skills")
        {
            Skills = new SkillSet();
            RelatedSkills = new SkillSet();
        }
        public bool HasError { get; set; }
        public string SearchTerm { get; set; }
        public SkillSet Skills { get; private set; }
        public SkillSet RelatedSkills { get; private set; }
    }
}
