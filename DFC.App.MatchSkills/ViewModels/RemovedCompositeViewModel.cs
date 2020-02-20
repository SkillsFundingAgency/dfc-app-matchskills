using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class RemovedCompositeViewModel : CompositeViewModel
    {
        public RemovedCompositeViewModel() : base(PageId.Removed, "You have successfully removed the following skills from your skills list")
        {
            Skills = new SkillSet();
        }

        public SkillSet Skills { get; set; }
    }
}
