using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;

namespace DFC.App.MatchSkills.Application.Dysac
{
    public interface IDysacSessionReader
    {
        Task<DysacServiceResponse> InitiateDysac(string sessionId="");
        Task<DysacJobCategory[]> GetDysacJobCategories(string sessionId);
    }
}
