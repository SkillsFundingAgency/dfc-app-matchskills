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
        private readonly ILogger<DysacService> _log;
        private IOptions<DysacSettings> _DysacSettings;
        private readonly IRestClient _client;
        public DysacService(ILogger<DysacService> log, IRestClient client, IOptions<DysacSettings> DysacSettings)
        {
            Throw.IfNull(DysacSettings, nameof(DysacSettings));
            _log = log;
            _client = client;
            _DysacSettings = DysacSettings;

        }

        public Task<DysacServiceResponse> InitiateDysac(string sessionId = null)
        {
            return sessionId == String.Empty
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
