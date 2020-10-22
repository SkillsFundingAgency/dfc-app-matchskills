using System.Linq;
using DFC.Personalisation.Domain.Models;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.MatchSkills.ViewModels
{
    public class OccupationSearchDetailsCompositeViewModel : CompositeViewModel
    {
        public OccupationSearchDetailsCompositeViewModel()
            : base(PageId.OccupationSearchDetails, "Select a Job")
        {
        }

        public Occupation[] Occupations { get; set; }

    }
}
