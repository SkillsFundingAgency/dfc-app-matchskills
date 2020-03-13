using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Helpers
{
    public static class SessionHelper
    {
        public static DysacService CreateNewDysacSession(HttpMessageHandler handler = null)
        {
            IOptions<DysacSettings> DysacSettings;
            DysacSettings = Options.Create(new DysacSettings());
            DysacSettings.Value.ApiUrl="http://localhost:7074/api/";
            DysacSettings.Value.ApiKey="ApiKey";
            DysacSettings.Value.DysacUrl = "DysacUrl";

            return new DysacService(log:Substitute.For<ILogger<DysacService>>(),
                handler == null ? new RestClient() : new RestClient(handler), DysacSettings
                );
            
        }
        public static DysacService CreateNewDysacSession_Invalid_RestClient()
        {
            IOptions<DysacSettings> DysacSettings;
            DysacSettings = Options.Create(new DysacSettings());
            DysacSettings.Value.ApiUrl="http://localhost:7074/api/";
            DysacSettings.Value.ApiKey="ApiKey";
            DysacSettings.Value.DysacUrl = "DysacUrl";
            return new DysacService(log:Substitute.For<ILogger<DysacService>>(), 
                null,
                DysacSettings);
        }
    }
}
