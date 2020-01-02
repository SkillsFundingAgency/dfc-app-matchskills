using System.Threading.Tasks;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Application.ServiceTaxonomy
{
    public interface IServiceTaxonomyReader
    {
        Task<Skill[]> GetAllSkills<TSkills>(string apiPath, string ocpApimSubscriptionKey);
        Task<Occupation[]> GetAllOccupations<TOccupations>(string apiPath, string ocpApimSubscriptionKey);
    }
}
