using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Helpers
{
    public static class SessionHelper
    {
        public static DysacService CreateNewDysacSession(HttpMessageHandler handler = null)
        {
            
            return new DysacService(log:Substitute.For<ILogger>(), new DysacServiceSettings
            {
                ApiUrl = "http://localhost:7074/api/", 
                ApiKey = "ApiKey"
            }, handler == null ?  new RestClient() : new RestClient(handler));
        }
        public static DysacService CreateNewDysacSession_Invalid_RestClient()
        {
            
            return new DysacService(log:Substitute.For<ILogger>(), new DysacServiceSettings
                {
                    ApiUrl = "http://localhost:7074/api/", 
                    ApiKey = "ApiKey"
                }, null);
        }
    }
}
