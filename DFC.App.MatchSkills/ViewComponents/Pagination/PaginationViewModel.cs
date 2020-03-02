namespace DFC.App.MatchSkills.ViewComponents.Pagination
{
    public class PaginationViewModel
    {
        public int TotalPages { get; set; }
        public int? NextPage { get; set; }
        public string NextPageLink { get; set; }
        public int? PreviousPage { get; set; }
        public string PreviousPageLink { get; set; }
        public string ShowResultsString { get; set; }
    }
}
