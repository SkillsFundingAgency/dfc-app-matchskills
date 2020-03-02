using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.LMI.Interfaces;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Application.LMI.Services
{
    public class LmiService : ILmiService
    {
        private readonly RestClient _restClient;
        private readonly IOptions<LmiSettings> _settings;

        public LmiService(RestClient restClient, IOptions<LmiSettings> settings)
        {
            _restClient = restClient ?? new RestClient();
            Throw.IfNullOrWhiteSpace(settings.Value.ApiUrl, nameof(settings.Value.ApiUrl));
            _settings = settings;
        }
        public async Task<IList<OccupationMatch>> GetPredictionsForGetOccupationMatches(IList<OccupationMatch> matches)
        {
            if(matches == null || matches.Count == 0)
                return matches;
            foreach (var match in matches)
            {

                var prediction = await GetPredictionsForSocCode(match.SocCode, PredictionFilter.Region);
                if(prediction != null)
                    match.JobGrowth = DetermineJobSectorGrowth(prediction);
            }

            return matches;
        }
        public async Task<WfPredictionResult> GetPredictionsForSocCode(int socCode, PredictionFilter filter)
        {
            if (socCode <= 0)
                return null;
            var request = new WfPredictionRequest()
            {
                FilterType = filter,
                SocCode = socCode
            };
            var postData = new StringContent($"{JsonConvert.SerializeObject(request)}", Encoding.UTF8, MediaTypeNames.Application.Json);
            return await _restClient.PostAsync<WfPredictionResult>($"{_settings.Value.ApiUrl}/wf/predict/breakdown/{filter}?soc={socCode}", postData);
        }

        internal JobGrowth DetermineJobSectorGrowth(WfPredictionResult result)
        {
            if (result.PredictedEmployment == null || result.PredictedEmployment.Length == 0) 
                return JobGrowth.Undefined;

            var year = DateTime.UtcNow.Year;
            var currentYearTotal = CalculateTotal(result, year);
            var previousYearTotal = CalculateTotal(result, year - 1);
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
