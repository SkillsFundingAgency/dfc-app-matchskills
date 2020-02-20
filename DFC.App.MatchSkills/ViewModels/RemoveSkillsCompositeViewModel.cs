using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class RemoveSkillsCompositeViewModel : CompositeViewModel
    {
        public RemoveSkillsCompositeViewModel() : base(PageId.RemoveSkills, "Remove Skills")
        {
            Skills = new SkillSet();
        }

        public SkillSet Skills { get; set; }
        public bool HasError { get; set; }
    }
}
