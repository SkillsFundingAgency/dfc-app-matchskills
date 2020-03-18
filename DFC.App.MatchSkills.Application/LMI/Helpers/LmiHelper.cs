using System;
using System.Linq;
using DFC.App.MatchSkills.Application.LMI.Models;

namespace DFC.App.MatchSkills.Application.LMI.Helpers
{
    public static class LmiHelper
    {

        public static JobGrowth DetermineJobSectorGrowth(WfPredictionResult result)
        {
            if (result.PredictedEmployment == null || result.PredictedEmployment.Length == 0)
                return JobGrowth.Undefined;

            var year = DateTime.UtcNow.Year;
            var currentYearTotal = CalculateTotal(result, year);
            var previousYearTotal =
                CalculateTotal(result, result.PredictedEmployment.Select(x => x.Year).AsEnumerable().First());
            return currentYearTotal > previousYearTotal ? JobGrowth.Increasing : JobGrowth.Decreasing;
        }

        public static Breakdown[] RemoveUnwantedRegions(Breakdown[] predictedEmployment)
        {
            var alteredList = predictedEmployment.ToList();
            alteredList.RemoveAll(x => x.Code == Region.Wales ||
                                       x.Code == Region.Scotland ||
                                       x.Code == Region.NorthernIreland);
            return alteredList.ToArray();
        }

        public static int CalculateTotal(WfPredictionResult predictedEmployment, int year)
        {
            var yearBreakdown = RemoveUnwantedRegions(predictedEmployment.PredictedEmployment.Where(x => x.Year == year)
                .Select(x => x.Breakdown).FirstOrDefault());
            return yearBreakdown.Sum(x => x.Employment);
        }

        public static bool IsOutOfDate(DateTimeOffset lastCheckedDate, int cacheLifespan)
        {
            var expiryDate = lastCheckedDate.AddDays(cacheLifespan);
            var dateNow = DateTimeOffset.Now;
            if (dateNow > expiryDate)
            {
                return true;
            }
            return false;
        }
    }
}
