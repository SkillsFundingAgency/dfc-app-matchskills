namespace DFC.App.MatchSkills.ViewModels
{
    public class OccupationSearchViewModel : CompositeViewModel
    {
        public OccupationSearchViewModel()
            : base(PageId.OccupationSearch, "Enter your current or previous job title")
        {
        }

        public string SearchService { get; set; }
    }
}
