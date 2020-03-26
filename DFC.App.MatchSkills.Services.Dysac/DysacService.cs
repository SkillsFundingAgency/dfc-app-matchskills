using Castle.Core.Internal;
using Dfc.ProviderPortal.Packages;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.MatchSkills.Services.Dysac
{

    public class DysacService : IDysacSessionReader, IDysacSessionWriter
    {
        
        private readonly IOptions<DysacSettings> _dysacSettings;
        private readonly IRestClient _client;
        private readonly ILogger _logger;
        private const string ResultsEndpoint = "/result/";
        public DysacService(ILogger<DysacService> log, IRestClient client, IOptions<DysacSettings> dysacSettings)
        {
            Throw.IfNull(dysacSettings, nameof(dysacSettings));
            _logger = log;
            _client = client;
            _dysacSettings = dysacSettings;
        }


        public Task<DysacServiceResponse> InitiateDysac()
        {
            var serviceUrl = $"{_dysacSettings.Value.ApiUrl}assessment/short";
            var response = _client.PostAsync<Task<int>>(serviceUrl,new StringContent(""));
            
            return response.Result.Result.Equals(DysacReturnCode.Ok) 
                ? Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Ok})
                : Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Error,ResponseMessage = response.ToString()});
        }

        public Task<DysacServiceResponse> InitiateDysac(string sessionId)
        {
            Throw.IfNull(sessionId, nameof(sessionId));
            var serviceUrl = $"{_dysacSettings.Value.ApiUrl}assessment/session/{sessionId}";

            var response = _client.GetAsync<Task<int>>(serviceUrl);
            
            return response.Result.Result.Equals(DysacReturnCode.Ok) 
                ? Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Ok})
                : Task.FromResult(new DysacServiceResponse() {ResponseCode = DysacReturnCode.Error,ResponseMessage = response.ToString()});
            
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
                var response = await _client.GetAsync<DysacResults>(serviceUrl);
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
