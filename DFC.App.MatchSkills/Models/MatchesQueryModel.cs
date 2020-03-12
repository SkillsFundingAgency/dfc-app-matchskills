using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.Models
{
    public class MatchesFilterModel
    {
        public int Page { get; set; }
        public SortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
