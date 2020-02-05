namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Models
{
    public class ServiceTaxonomySettings
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }

        public string SearchOccupationInAltLabels { get; set; }
        public string SearchSkillInAltLabels { get; set; }
      
        public string SearchService { get; set; }
        public string EscoUrl { get; set; }
        
    }
}
