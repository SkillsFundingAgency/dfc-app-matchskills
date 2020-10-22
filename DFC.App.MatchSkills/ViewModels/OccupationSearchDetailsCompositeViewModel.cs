using DFC.Personalisation.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.MatchSkills.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class OccupationSearchDetailsCompositeViewModel : CompositeViewModel
    {
        public OccupationSearchDetailsCompositeViewModel()
            : base(PageId.OccupationSearchDetails, "Select a Job")
        {
        }

        public Occupation[] Occupations { get; set; }

    }
}
