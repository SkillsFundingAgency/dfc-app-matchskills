namespace DFC.App.MatchSkills.ViewModels
{
    public class WorkedCompositeViewModel : CompositeViewModel
    {
        public bool HasError { get; set; }
        public WorkedCompositeViewModel()
            : base(PageId.Worked, "Have you worked before?")
        {
        }
    }
}
