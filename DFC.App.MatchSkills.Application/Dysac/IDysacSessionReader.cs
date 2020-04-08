using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using Dfc.Session.Models;

namespace DFC.App.MatchSkills.Application.Dysac
{
    public interface IDysacSessionReader
    {
        Task<DysacServiceResponse> InitiateDysacOnly();
        Task<DysacServiceResponse> InitiateDysac(DfcUserSession userSession);
        Task<DysacJobCategory[]> GetDysacJobCategories(string sessionId);
        Task<DysacServiceResponse> LoadExistingDysacOnlyAssessment(string sessionId);
    }
}
