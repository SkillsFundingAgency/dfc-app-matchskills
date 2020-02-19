using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class EnterSkillsCompositeViewModel : CompositeViewModel
    {
        public EnterSkillsCompositeViewModel() : base(PageId.EnterSkills, "Search for a skill")
        {
            Skills = new SkillSet();
        }
        public bool HasError { get; set; }
        public SkillSet Skills { get; private set; }
    }
}
