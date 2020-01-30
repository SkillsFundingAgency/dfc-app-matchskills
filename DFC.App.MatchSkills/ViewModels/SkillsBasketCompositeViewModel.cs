using System.Collections.Generic;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class SkillsBasketCompositeViewModel : CompositeViewModel
    {
        public ICollection<Skill> Skills { get; private set; }

        public SkillsBasketCompositeViewModel()
            : base(PageId.SkillsBasket, "Your skills list")
        {
            Skills = new List<Skill>();
        }
    }
}
