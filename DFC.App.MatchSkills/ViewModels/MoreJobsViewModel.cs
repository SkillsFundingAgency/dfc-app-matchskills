namespace DFC.App.MatchSkills.ViewModels
{
    public class MoreJobsViewModel : CompositeViewModel
    {
        public MoreJobsViewModel() : base(PageId.MoreJobs, "Enter your job title")
        {
        }
        public string SearchService { get; set; }
    }
}
