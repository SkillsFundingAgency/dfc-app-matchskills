using Castle.Core.Internal;
using Dfc.ProviderPortal.Packages;
using Dfc.Session;
using Dfc.Session.Models;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.Dysac
{

    public class DysacService : IDysacSessionReader, IDysacSessionWriter
    {
        
        private readonly IOptions<DysacSettings> _dysacSettings;
        private readonly IRestClient _restClient;
        private readonly ILogger _logger;
        private const string ResultsEndpoint = "/result/";
        private readonly ISessionClient _sessionClient;

        public DysacService(ILogger<DysacService> log, IRestClient restClient, IOptions<DysacSettings> dysacSettings, ISessionClient sessionClient)
        {
            Throw.IfNull(dysacSettings, nameof(dysacSettings));
            _logger = log;
            _dysacSettings = dysacSettings;
            _restClient = restClient ?? new RestClient();
            _sessionClient = sessionClient;
        }


        public async Task<DysacServiceResponse> InitiateDysac()
        {
            var serviceUrl = $"{_dysacSettings.Value.ApiUrl}assessment/short";
            var request = new HttpRequestMessage();
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dysacSettings.Value.ApiKey);
            request.Headers.Add("version", _dysacSettings.Value.ApiVersion);

           var response = await _restClient.PostAsync<AssessmentShortResponse>(serviceUrl, request);

           var dysacServiceResponse = new DysacServiceResponse();
           if (response.SessionId != "")
           {
               dysacServiceResponse.ResponseCode = DysacReturnCode.Ok;
               var userSession = new DfcUserSession()
               {
                   CreatedDate = response.CreatedDate,
                   PartitionKey = response.PartitionKey,
                   Salt =response.Salt,
                   SessionId = response.SessionId
               };
               _sessionClient.CreateCookie(userSession,false);

           }
           else
           {
               dysacServiceResponse.ResponseCode = DysacReturnCode.Error;
               dysacServiceResponse.ResponseMessage = response.ToString();
           }

           return dysacServiceResponse;

        }

        public async Task<DysacServiceResponse> InitiateDysac(DfcUserSession userSession)
        {
            Throw.IfNull(userSession, nameof(userSession));
            var serviceUrl = $"{_dysacSettings.Value.ApiUrl}assessment/skills";
            var request = new HttpRequestMessage();
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dysacSettings.Value.ApiKey);
            request.Headers.Add("version", _dysacSettings.Value.ApiVersion);

            request.Content = new StringContent($"{{\"PartitionKey\":\"{userSession.PartitionKey}\"," +
                                                $"\"SessionId\":\"{userSession.SessionId}\"," +
                                                $"\"Salt\":\"{userSession.Salt}\"," +
                                                $"\"CreatedDate\":{JsonConvert.SerializeObject(userSession.CreatedDate)}}}",
                Encoding.UTF8, "application/json");

            

            try
            {
                await _restClient.PostAsync<AssessmentShortResponse>(serviceUrl, request);
                return _restClient.LastResponse.StatusCode == HttpStatusCode.Created || _restClient.LastResponse.StatusCode == HttpStatusCode.AlreadyReported
                    ? (new DysacServiceResponse() {ResponseCode = DysacReturnCode.Ok})
                    : (new DysacServiceResponse() {ResponseCode = DysacReturnCode.Error,ResponseMessage = _restClient.LastResponse.StatusCode.ToString()});
            }
            catch 
            {
                return new DysacServiceResponse()
                {
                    ResponseCode = DysacReturnCode.Error,
                    ResponseMessage = _restClient.LastResponse.StatusCode.ToString()
                };
            }

            
            
        }

        public async Task<DysacJobCategory[]> GetDysacJobCategories(string sessionId)
        {
            if (sessionId.IsNullOrEmpty())
            {
                return null;
            }
            var serviceUrl = $"{_dysacSettings.Value.ApiUrl}{ResultsEndpoint}/{sessionId}/short";
            try
            {
                var response = await _restClient.GetAsync<DysacResults>(serviceUrl);
                if (response != null && response.JobCategories.Any())
                {
                    return Mapping.Mapper.Map<DysacJobCategory[]>(response.JobCategories);
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Information,$"{e.Message} SessionId: {sessionId}");
                return null;
            }


            return new DysacJobCategory[0];

        }
        
        
    }

   
}
