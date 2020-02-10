namespace DFC.App.MatchSkills.Application.Session.Models
{
    public class CreateSessionRequest
    {
        public string PreviousPage { get; set; }
        public string CurrentPage { get; set; }
        public bool? UserHasWorkedBefore { get; set; }
        public bool? RouteIncludesDysac { get; set; }
    }
}
