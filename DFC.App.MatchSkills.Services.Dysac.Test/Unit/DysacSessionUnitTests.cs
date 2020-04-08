using Dfc.Session;
using Dfc.Session.Models;
using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacSessionUnitTests
    {

        public class CreateNewSessionTests
        {
            private IOptions<DysacSettings> _dysacServiceSetings;
            private IDysacSessionReader _dysacService;
            private ISessionClient _sessionClient;
            private IRestClient _restClient;
            private ILogger<DysacService> _log;
            [SetUp]
            public void Init()
            {

                _dysacServiceSetings = Options.Create(new DysacSettings());
                _dysacServiceSetings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/something";
                _dysacServiceSetings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
                _dysacServiceSetings.Value.DysacUrl = "http://dysacurl";
                _dysacServiceSetings.Value.ApiVersion = "v1";
                _dysacService = Substitute.For<IDysacSessionReader>();
                _log = Substitute.For<ILogger<DysacService>>();
                _sessionClient = Substitute.For<ISessionClient>();
                _restClient = Substitute.For<IRestClient>();

            }

            [Test]
            public void When_DysacService_ObjectCreated()
            {

                var logger = Substitute.For<ILogger<DysacService>>();
                var restClient = Substitute.For<IRestClient>();
                var dysacService = new DysacService(logger, restClient, _dysacServiceSetings, _sessionClient);
            }

            [Test]
            
            public void When_InitiateDysacNewSessionWithNoErrors_ReturnOK()
            {
                
                           
                var userSession = new DfcUserSession();
                userSession.PartitionKey = "key";
                
                var restClient = Substitute.For<IRestClient>();

                var lastResponse = Substitute.For<RestClient.APIResponse>(new HttpResponseMessage(){Content = new StringContent("something",Encoding.UTF8),StatusCode = HttpStatusCode.Created});
                
                restClient.LastResponse.Returns(lastResponse);
                
                restClient.PostAsync<AssessmentShortResponse>(apiPath:"",content:null).ReturnsForAnyArgs(new AssessmentShortResponse()
                {
                    CreatedDate = DateTime.Now,
                    SessionId = "sesionId",
                    Salt = "salt",
                    PartitionKey = "p-key"
                });

                IDysacSessionReader dysacService = new DysacService(_log,restClient,_dysacServiceSetings,_sessionClient); 
                var results = dysacService.InitiateDysac(userSession).Result;
                
                results.ResponseCode.Should().Be(DysacReturnCode.Ok);
                
            }

            [Test]
            public void When_InitiateDysacWithErrors_ReturnErrorAndMessage()
            {
                _dysacService.InitiateDysacOnly().ReturnsForAnyArgs(new DysacServiceResponse()
                {
                    ResponseCode = DysacReturnCode.Error,
                    ResponseMessage = "Error"
                });

                var results = _dysacService.InitiateDysacOnly().Result;
                results.ResponseCode.Should().Be(DysacReturnCode.Error);
                results.ResponseMessage.Should().Be("Error");
            }


            [Test]
            public async Task When_LoadExistingDysacOnlyAssessmentReturnsValidResponse_ReturnOK()
            {
                _restClient.GetAsync<AssessmentShortResponse>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ReturnsForAnyArgs(new AssessmentShortResponse()
                {
                    CreatedDate = DateTime.Now,
                    PartitionKey = "partitionkey",
                    SessionId = "session",
                    Salt = "salt"
                });
                var dysacService = new DysacService(_log, _restClient, _dysacServiceSetings, _sessionClient);

                var results = await dysacService.LoadExistingDysacOnlyAssessment("session");
                results.ResponseCode.Should().Be(DysacReturnCode.Ok);
            }

            [Test]
            public async Task When_LoadExistingDysacOnlyAssessmentReturnsAnError_ReturnError()
            {
                _restClient.GetAsync<AssessmentShortResponse>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ReturnsNullForAnyArgs();
                var dysacService = new DysacService(_log, _restClient, _dysacServiceSetings, _sessionClient);
                var results = await dysacService.LoadExistingDysacOnlyAssessment("session");
                results.ResponseCode.Should().Be(DysacReturnCode.Error);
            }

        }




    }
}
