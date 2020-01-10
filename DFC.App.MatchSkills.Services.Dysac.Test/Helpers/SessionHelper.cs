using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Helpers
{
    public static class SessionHelper
    {
        public static DysacService CreateNewDysacSession(HttpMessageHandler handler = null)
        {
            
            return new DysacService(log:Substitute.For<ILogger>(),
                handler == null ? new RestClient() : new RestClient(handler),
                new DysacServiceSettings
                {
                    ApiUrl = "http://localhost:7074/api/", 
                    ApiKey = ""
                });
        }
        public static DysacService CreateNewDysacSession_Invalid_RestClient()
        {
            
            return new DysacService(log:Substitute.For<ILogger>(), 
                null,
                new DysacServiceSettings
                {
                    ApiUrl = "http://localhost:7074/api/", 
                    ApiKey = ""
                });
        }
    }
}
