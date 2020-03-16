using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.LMI.Interfaces;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Extensions;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.Cosmos.Models;

namespace DFC.App.MatchSkills.Application.LMI.Services
{
    public class LmiService : ILmiService
    {
        private readonly IRestClient _restClient;
        private readonly ICosmosService _cosmosService;
        private readonly IOptions<LmiSettings> _lmiSettings;

        public LmiService(IOptions<LmiSettings> lmiSettings, ICosmosService cosmosService)
        {
            _restClient = new RestClient();
            _cosmosService = cosmosService;
            Throw.IfNullOrWhiteSpace(lmiSettings.Value.ApiUrl, nameof(lmiSettings.Value.ApiUrl));
            _lmiSettings = lmiSettings;
        }
        public LmiService(IRestClient restClient, IOptions<LmiSettings> settings, ICosmosService cosmosService)
        {
            _restClient = restClient ?? new RestClient();
            _cosmosService = cosmosService;
            Throw.IfNullOrWhiteSpace(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            _lmiSettings = settings;
        }
        public IList<OccupationMatch> GetPredictionsForGetOccupationMatches(IList<OccupationMatch> matches)
        {
            if(matches == null || matches.Count == 0)
                return matches;

            var tasks = new List<Task>();
            foreach (var match in matches)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var cachedResult = CheckCachedLmiData(match.SocCode);
                    if (cachedResult == JobGrowth.Undefined)
                    {
                        var prediction = await GetPredictionsForSocCode(match.SocCode, PredictionFilter.Region);
                        if (prediction != null)
                            match.JobGrowth = DetermineJobSectorGrowth(prediction);
                    }
                    else
                    {
                        match.JobGrowth = cachedResult;
                    }

                }));


            }

            var t = Task.WhenAll(tasks);
            t.Wait();
            return matches;
        }

        
        public async Task<WfPredictionResult> GetPredictionsForSocCode(int socCode, PredictionFilter filter)
        {
            if (socCode <= 0)
                return null;
            try
            {
                return await _restClient.GetAsync<WfPredictionResult>(
                    $"{_lmiSettings.Value.ApiUrl}/wf/predict/breakdown/{filter.ToLower()}?soc={socCode}");
            }
            catch
            {
                return null;
            }

        }

        internal JobGrowth CheckCachedLmiData(int socCode)
        {
            
            return JobGrowth.Undefined;
        }
        internal JobGrowth DetermineJobSectorGrowth(WfPredictionResult result)
        {
            if (result.PredictedEmployment == null || result.PredictedEmployment.Length == 0) 
                return JobGrowth.Undefined;

            var year = DateTime.UtcNow.Year;
            var currentYearTotal = CalculateTotal(result, year);
            var previousYearTotal =
                CalculateTotal(result, result.PredictedEmployment.Select(x => x.Year).AsEnumerable().First());
            return currentYearTotal > previousYearTotal ? JobGrowth.Increasing : JobGrowth.Decreasing;
        }

        internal Breakdown[] RemoveUnwantedRegions(Breakdown[] predictedEmployment)
        {
            var alteredList = predictedEmployment.ToList();
            alteredList.RemoveAll(x => x.Code == Region.Wales || 
                                       x.Code == Region.Scotland || 
                                       x.Code == Region.NorthernIreland);
            return alteredList.ToArray();
        }

        internal int CalculateTotal(WfPredictionResult predictedEmployment, int year)
        {
            var yearBreakdown = RemoveUnwantedRegions(predictedEmployment.PredictedEmployment.Where(x => x.Year == year)
                .Select(x => x.Breakdown).FirstOrDefault());
            return yearBreakdown.Sum(x => x.Employment);
        }


        
    }
}
