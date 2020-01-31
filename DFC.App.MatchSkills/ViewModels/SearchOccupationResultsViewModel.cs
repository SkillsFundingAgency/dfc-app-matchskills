using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class SearchOccupationResultsViewModel
    {
        public string Title { get; set; }
        public Occupation[] Occupations { get; set; }
    }
}
