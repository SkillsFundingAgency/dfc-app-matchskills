using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Extensions;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            _getCreateDysacSessionUri = DysacServiceSettingsExtensions.GetCreateDysacSessionUri(dysacApiSettings);
            _client = client;
        }
        public async Task<NewSessionResponse> CreateNewSession(string assessmentType)
        {
            if (string.IsNullOrWhiteSpace(assessmentType))
            {
                var ex = new ArgumentException("Null or empty assessment type passed"); 
                _log.LogError(ex.Message, ex);
                throw ex;
            }

            if (assessmentType.ToEnum(AssessmentTypes.Undefined) == AssessmentTypes.Undefined)
            {
                var ex = new ArgumentException("Invalid value passed"); 
                _log.LogError(ex.Message, ex);
                throw ex;
            }

            var requestJson = JsonConvert.SerializeObject(new NewSessionRequest
            {
                AssessmentType = assessmentType
            });

            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            SetDssCorrelationId();
            try
            {
                var response =
                    await _client.PostAsync(_getCreateDysacSessionUri.AbsoluteUri + $"?assessmentType={assessmentType}",
                        content);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<NewSessionResponse>(
                        response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                throw;
            }

            return null;

        }

        internal void SetDssCorrelationId()
        {
            _client.DefaultRequestHeaders.Remove("DssCorrelationId");
            var correlationId = Guid.NewGuid();
            _client.DefaultRequestHeaders.Add("DssCorrelationId", correlationId.ToString());
        }
    }



    internal static class DysacServiceSettingsExtensions
    {
        internal static Uri GetCreateDysacSessionUri(this DysacServiceSettings extendee)
        {
            var uri = new Uri(extendee.ApiUrl);
            var key = extendee.ApiKey;
            var trimmed = uri.AbsoluteUri.TrimEnd('/');
            return new Uri($"{trimmed}/assessment");
        }
    }
}
