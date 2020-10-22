namespace DFC.App.MatchSkills.ViewModels
{
    public class OccupationSearchCompositeViewModel : CompositeViewModel
    {
        public OccupationSearchCompositeViewModel()
            : base(PageId.OccupationSearch, "Enter your current or previous job title")
        {
        }

        public string SearchService { get; set; }
        public bool HasError { get; set; }
        public string AutoCompleteElementName { get;} = "OccupationAutoComplete";
        public string FormElementName { get; } = "OccupationSearch";
        public bool HasErrors { get; set; }
    }
}
