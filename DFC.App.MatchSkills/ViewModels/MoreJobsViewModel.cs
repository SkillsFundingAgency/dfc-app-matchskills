using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class MoreJobsViewModel : CompositeViewModel
    {
        public string SearchService { get; set; }
        public bool HasError { get; set; }
        public string AutoCompleteElementName { get; } = "OccupationSearchAutoComplete";
        public string FormElementName { get; } = "OccupationSearch";
        public OccupationSet Occupations { get; private set; }

        public MoreJobsViewModel() : base(PageId.OccupationSearch, "Enter your job title")
        {
            Occupations = new OccupationSet();
        }

    }
}
