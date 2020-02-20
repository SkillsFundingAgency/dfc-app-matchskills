using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class ConfirmRemoveCompositeViewModel : CompositeViewModel
    {
        public ConfirmRemoveCompositeViewModel() : base(PageId.ConfirmRemove, "Do you want to remove the displayed skills from your skills list?")
        {
            Skills = new SkillSet();
        }

        public SkillSet Skills { get; set; }
    }
}
