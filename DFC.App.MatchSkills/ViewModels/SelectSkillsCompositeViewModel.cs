using DFC.Personalisation.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.MatchSkills.ViewModels
{
    public class SelectSkillsCompositeViewModel : CompositeViewModel
    {
        public string Occupation { get; set; }
        public  List<Skill> Skills { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorSummaryMessage { get; set; }
        public bool HasError { get; set; }
        public bool AllSkillsSelected { get; set; }
        public SelectSkillsCompositeViewModel()  : base(PageId.SelectSkills, "Select your skills")
        {
            AllSkillsSelected = false;
        }

    }
}
