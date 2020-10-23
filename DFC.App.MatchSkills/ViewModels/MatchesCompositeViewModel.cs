using DFC.App.MatchSkills.Models;
using System.Collections.Generic;
using DFC.App.MatchSkills.Application.Session.Models;

namespace DFC.App.MatchSkills.ViewModels
{
    public class MatchesCompositeViewModel : CompositeViewModel
    {
        public ICollection<CareerMatch> CareerMatches { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string ResultsString { get; set; }
        public int TotalMatches { get; set; }
        public SortBy CurrentSortBy { get; set; }
        public SortDirection CurrentDirection { get; set; }
        public string DysacSaveUrl { get; set; }

        public MatchesCompositeViewModel()
            : base(PageId.Matches, "Your career matches")
        {
            CareerMatches = new List<CareerMatch>();
        }

        public SortDirection GetSortDirection(SortBy sortBy)
        {
            if (CurrentSortBy != sortBy) return sortBy == SortBy.MatchPercentage ? SortDirection.Descending : SortDirection.Ascending;

            return CurrentDirection == SortDirection.Descending ? SortDirection.Ascending : SortDirection.Descending;
        }
    }
}