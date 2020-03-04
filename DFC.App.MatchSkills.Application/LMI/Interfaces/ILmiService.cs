using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.LMI.Interfaces
{
    public interface ILmiService
    {
        IList<OccupationMatch> GetPredictionsForGetOccupationMatches(IList<OccupationMatch> matches);
    }
}
