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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;


namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacSessionUnitTests
    {

        public class CreateNewSessionTests
        {
            private IOptions<DysacSettings> _dysacServiceSetings;
            private IOptions<OldDysacSettings> _oldDysacServiceSetings;
            private IDysacSessionReader _dysacService;
            private ISessionClient _sessionClient;
            private IRestClient _restClient;
            private ILogger<DysacService> _log;
            [SetUp]
            public void Init()
            {

                _oldDysacServiceSetings = Options.Create(new OldDysacSettings()
                {
                    ApiKey = "9238dfjsjdsidfs83fds",
                    AssessmentApiUrl = "https://this.is.anApi.org.uk",
                    DysacResultsUrl = "https://this.is.anApi.org.uk",
                });
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
                var dysacService = new DysacService(logger, restClient, _dysacServiceSetings, _oldDysacServiceSetings, _sessionClient);
            }

            [Test]
            
            public async Task When_InitiateDysacNewSessionWithNoErrors_ReturnOK()
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

                IDysacSessionReader dysacService = new DysacService(_log,restClient,_dysacServiceSetings,_oldDysacServiceSetings, _sessionClient);
                await dysacService.InitiateDysac(userSession);

            }

            [Test]
            public async Task When_InitiateDysacWithErrors_ReturnErrorAndMessage()
            {
                await _dysacService.InitiateDysacOnly();
                
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

                var lastResponse = Substitute.For<RestClient.APIResponse>(new HttpResponseMessage(){Content = new StringContent("something",Encoding.UTF8),StatusCode = HttpStatusCode.Created});
                
                _restClient.LastResponse.Returns(lastResponse);

                var dysacService = new DysacService(_log, _restClient, _dysacServiceSetings,_oldDysacServiceSetings,_sessionClient);


                await dysacService.LoadExistingDysacOnlyAssessment("session");
                
            }



            [Test]

            public async Task When_InitiateDysacOnlySessionWithNoErrors_ReturnOK()
            {
                var userSession = new DfcUserSession();
                userSession.PartitionKey = "key";
                
                var lastResponse = Substitute.For<RestClient.APIResponse>(new HttpResponseMessage() { Content = new StringContent("something", Encoding.UTF8), StatusCode = HttpStatusCode.Created });

                _restClient.LastResponse.Returns(lastResponse);

                _restClient.PostAsync<AssessmentShortResponse>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>())
                    .ReturnsForAnyArgs(new AssessmentShortResponse
                        {
                            CreatedDate = DateTime.Now,
                            SessionId = "sesionId",
                            Salt = "salt",
                            PartitionKey = "p-key"
                        }
                    );

                IDysacSessionReader dysacService = new DysacService(_log, _restClient, _dysacServiceSetings, _oldDysacServiceSetings, _sessionClient);
                await dysacService.InitiateDysacOnly();

            }

        }
    }
}
