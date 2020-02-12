using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class SkillsBasketCompositeViewModel : CompositeViewModel
    {
        public SkillSet Skills { get; private set; }

        public SkillsBasketCompositeViewModel()
            : base(PageId.SkillsBasket, "Your skills list")
        {
            Skills = new SkillSet();
        }
    }
}
