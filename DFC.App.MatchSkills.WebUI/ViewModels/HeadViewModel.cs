namespace DFC.App.MatchSkills.WebUI.ViewModels
{
    public class HeadViewModel
    {
        private const string DefaultPageTitle = "Discover your skills and careers";
        private const string DefaultCss = "https://dev-cdn.nationalcareersservice.org.uk/gds_service_toolkit/css/all.min.css";

        public string PageTitle { get; set; } = DefaultPageTitle;
        public string DefaultCssLink { get; set; } = DefaultCss;
    }
}


