namespace DFC.App.MatchSkills.ViewModels
{
    public class OccupationSearchResultsCompositeViewModel : CompositeViewModel
    {
        public OccupationSearchResultsCompositeViewModel()
            : base(PageId.OccupationSearch, "Occupation Search Results")
        {
        }

        public bool HasError { get; set; }
  
    }
}