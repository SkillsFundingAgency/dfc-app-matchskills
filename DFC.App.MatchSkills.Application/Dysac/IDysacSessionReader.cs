using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac.Models;
using Dfc.Session.Models;

namespace DFC.App.MatchSkills.Application.Dysac
{
    public interface IDysacSessionReader
    {
        void InitiateDysacOnly();
        void InitiateDysac(DfcUserSession userSession);
        Task<DysacJobCategory[]> GetDysacJobCategories(string sessionId);
        void LoadExistingDysacOnlyAssessment(string sessionId);
    }
}
