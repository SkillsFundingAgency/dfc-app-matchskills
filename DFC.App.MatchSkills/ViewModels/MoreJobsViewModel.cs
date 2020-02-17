using DFC.App.MatchSkills.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class MoreJobsViewModel : CompositeViewModel
    {
        public string SearchService { get; set; }
        public bool HasError { get; set; }
        public string AutoCompleteElementName { get; } = "MoreJobsAutoComplete";
        public string FormElementName { get; set; } = "";
        public OccupationSet Occupations { get; private set; }

        public MoreJobsViewModel() : base(PageId.MoreJobs, "Enter your job title")
        {
            Occupations = new OccupationSet();
        }

    }
}
