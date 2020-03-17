using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Cosmos.Interfaces;
using DFC.App.MatchSkills.Application.LMI.Helpers;
using DFC.App.MatchSkills.Application.LMI.Interfaces;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Extensions;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Cosmos.Services;

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
            Throw.IfLessThan(0, lmiSettings.Value.CacheLifespan, nameof(lmiSettings.Value.CacheLifespan));
            _lmiSettings = lmiSettings;
        }
        public LmiService(IRestClient restClient, IOptions<LmiSettings> lmiSettings, ICosmosService cosmosService)
        {
            _restClient = restClient ?? new RestClient();
            _cosmosService = cosmosService;
            Throw.IfNullOrWhiteSpace(lmiSettings.Value.ApiUrl, nameof(lmiSettings.Value.ApiUrl));
            Throw.IfLessThan(0, lmiSettings.Value.CacheLifespan, nameof(lmiSettings.Value.CacheLifespan));
            _lmiSettings = lmiSettings;
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
                    var cachedResult = await CheckCachedLmiData(match.SocCode);
                    if (cachedResult == JobGrowth.Undefined)
                    {
                        var prediction = await GetPredictionsForSocCode(match.SocCode, PredictionFilter.Region);
                        if (prediction != null)
                            match.JobGrowth = LmiHelper.DetermineJobSectorGrowth(prediction);
                        await CacheLmiData(match.SocCode, match.JobGrowth);
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
        internal async Task<JobGrowth> CheckCachedLmiData(int socCode)
        {
            if (socCode <= 0)
                return JobGrowth.Undefined;

            var result = await _cosmosService.ReadItemAsync(id:"", partitionKey: socCode.ToString(), CosmosCollection.LmiData);
            if (result.IsSuccessStatusCode)
            {
                var lmiData = JsonConvert.DeserializeObject<CachedLmiData>(await result.Content.ReadAsStringAsync());
                var isOutOfDate = LmiHelper.IsOutOfDate(lmiData.DateWritten, _lmiSettings.Value.CacheLifespan);
                if(!isOutOfDate)
                    return lmiData.JobGrowth;
            }

            return JobGrowth.Undefined;
        }

        internal async Task<HttpResponseMessage> CacheLmiData(int socCode, JobGrowth jobGrowth)
        {
            var cachedLmiData = new CachedLmiData
            {
                SocCode = socCode,
                JobGrowth = jobGrowth,
                DateWritten = DateTimeOffset.Now
            };
            return await _cosmosService.UpsertItemAsync(cachedLmiData, CosmosCollection.LmiData);
        }
    }
}
