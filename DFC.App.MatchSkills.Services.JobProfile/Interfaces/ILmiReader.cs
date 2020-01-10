using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.JobProfile.Models;

namespace DFC.App.MatchSkills.Services.JobProfile.Interfaces
{
    public interface ILmiReader
    {
        Task<IEnumerable<SocSearchResults>> SocSearch(SocSearchCriteria criteria);
        Task<WorkingFuturesSearchResults> WFSearch(WorkingFuturesRequest request);
    }
}
