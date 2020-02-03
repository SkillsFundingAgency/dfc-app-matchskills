using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class SelectSkillsCompositeViewModel : CompositeViewModel
    {
        public string Occupation;
        public ICollection<Skill> Skills;
        public SelectSkillsCompositeViewModel()  : base(PageId.SelectSkills, "Select your skills")
        {
            
        }

    }
}
