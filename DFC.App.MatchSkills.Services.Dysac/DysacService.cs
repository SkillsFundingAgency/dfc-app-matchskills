using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Extensions;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;

namespace DFC.App.MatchSkills.Services.Dysac
{
    
    public class DysacService : IDysacSessionReader, IDysacSessionWriter
    {
        private readonly ILogger _log;
        private readonly DysacServiceSettings _dysacApiSettings;
        private readonly Uri _getCreateDysacSessionUri;
        private readonly RestClient _client;
        public DysacService(ILogger log, DysacServiceSettings dysacApiSettings, RestClient client)
        {
            _log = log;
            _dysacApiSettings = dysacApiSettings;
            _getCreateDysacSessionUri = DysacServiceSettingsExtensions.GetCreateDysacSessionUri(_dysacApiSettings);
            _client = client;
        }
        // Edit to assessment type
        public async Task<NewSessionResponse> CreateNewSession(AssessmentTypes assessmentType)
        {

            try
            {
                var stubbedContent = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
                SetDssCorrelationId();

                return await _client.Post<NewSessionResponse>(_getCreateDysacSessionUri.AbsoluteUri + assessmentType.ToLower(), stubbedContent);
            }
            catch (HttpRequestException hre)
            {
                _log.LogError(hre.Message, hre);
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }

        }

        internal void SetDssCorrelationId()
        {
            _client.DefaultRequestHeaders.Remove(Constants.DssCorrelationIdHeader);
            var correlationId = Guid.NewGuid();
            _client.DefaultRequestHeaders.Add(Constants.DssCorrelationIdHeader, correlationId.ToString());
        }
    }



    internal static class DysacServiceSettingsExtensions
    {
        internal static Uri GetCreateDysacSessionUri(this DysacServiceSettings extendee)
        {
            var uri = new Uri(extendee.ApiUrl);
            var key = extendee.ApiKey;
            var trimmed = uri.AbsoluteUri.TrimEnd('/');
            return new Uri($"{trimmed}{Constants.CreateNewAssessmentPath}{Constants.CreateNewAssessmentQueryString}");
        }
    }

    internal static class Constants
    {
        internal const string DssCorrelationIdHeader = "DssCorrelationId";
        internal const string CreateNewAssessmentPath = "/assessment";
        internal const string CreateNewAssessmentQueryString = "?assessmentType=";
    }
}
