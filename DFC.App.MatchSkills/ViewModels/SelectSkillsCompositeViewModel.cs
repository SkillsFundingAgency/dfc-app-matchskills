using DFC.Personalisation.Domain.Models;
using System.Collections.Generic;

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
