using DFC.App.MatchSkills.Services.JobProfile.Models;
using DFC.App.MatchSkills.Services.JobProfile.Services;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http;

namespace DFC.App.MatchSkills.Services.JobProfile.Test.Helpers
{
    public class LmiHelper
    {
        public static class SocSearchRequests
        {
            public static SocSearchCriteria ValidSearchCriteria()
            {
                return new SocSearchCriteria
                {
                    SearchCriteria = "Developer"
                };
            }
        }

        public static class WfPredictSearchRequests
        {
            public static WorkingFuturesRequest ValidSearchCriteria()
            {
                return new WorkingFuturesRequest
                {
                    SocCode = 2136
                };
            }
        }
        public static LmiService LmiService_RestClient(HttpMessageHandler handler = null)
        {
            
            return new LmiService(log:Substitute.For<ILogger>(), new LmiApiSettings
            {
                ApiUrl = "http://api.lmiforall.org.uk/api/v1/",
            }, handler == null ?  new RestClient() : new RestClient(handler));
        }
    }
}
