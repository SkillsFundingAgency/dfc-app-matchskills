using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Application.ServiceTaxonomy
{
    public interface IServiceTaxonomySearcher
    {
        Task<Skill[]> SearchSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey,string skill);
        Task<Occupation[]> SearchOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey,string occupation,bool matchAltLabels);
        Task<Skill[]> GetAllSkillsForOccupation<TSkills>(string apiPath, string ocpApimSubscriptionKey, string occupation);
        Task<Skill[]> GetSkillsByLabel<TSkills>(string apiPath, string ocpApimSubscriptionKey, string label);
    }
}
