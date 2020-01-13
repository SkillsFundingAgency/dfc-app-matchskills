using DFC.App.MatchSkills.Services.JobProfile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.JobProfile.Interfaces
{
    public interface ILmiReader
    {
        Task<IEnumerable<SocSearchResults>> SocSearch(SocSearchCriteria criteria);
        Task<WorkingFuturesSearchResults> WFSearch(WorkingFuturesRequest request);
    }
}
