using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Dfc.ProviderPortal.Packages;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Services.Dysac
{

    public class DysacService : IDysacSessionReader, IDysacSessionWriter
    {
        
        private readonly IOptions<DysacSettings> _dysacSettings;
        private readonly IRestClient _client;
        public DysacService(ILogger<DysacService> log, IRestClient client, IOptions<DysacSettings> dysacSettings)
        {
            Throw.IfNull(dysacSettings, nameof(dysacSettings));
            _client = client;
            _dysacSettings = dysacSettings;
        }

        public Task<DysacServiceResponse> InitiateDysac(string sessionId = null)
        {
            var serviceUrl = _dysacSettings.Value.ApiUrl;
            var response = _client.GetAsync<Task<int>>(serviceUrl);
            
            //Handle response here and modify DysacServiceResponseAccordingly. Only returnig test responses for now so we can 
            //test both OK and error
            return String.IsNullOrEmpty(sessionId)
                ? Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Ok})
                : Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Error}); //DevOnly; 
            
        }

    }



    internal static class DysacSettingsExtensions
    {
        internal static Uri GetCreateDysacSessionUri(this DysacSettings extendee)
        {
            var uri = new Uri(extendee.ApiUrl);
            var trimmed = uri.AbsoluteUri.TrimEnd('/');
            return new Uri($"{trimmed}{Constants.CreateNewAssessmentPath}{Constants.CreateNewAssessmentQueryString}");
        }
    }

    internal static class Constants
    {
        internal const string DssCorrelationIdHeader = "DssCorrelationId";
        internal const string CreateNewAssessmentPath = "/assessments/api/assessment/";
        internal const string CreateNewAssessmentQueryString = "?assessmentType=";
    }
}
