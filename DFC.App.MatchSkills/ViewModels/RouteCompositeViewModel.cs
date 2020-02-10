namespace DFC.App.MatchSkills.ViewModels
{
    public class RouteCompositeViewModel : CompositeViewModel
    {
        public RouteCompositeViewModel()
            : base(PageId.Route, "What would you like your career matches to be based on?")
        {
        }

        public bool HasError { get; set; }
    }
}
