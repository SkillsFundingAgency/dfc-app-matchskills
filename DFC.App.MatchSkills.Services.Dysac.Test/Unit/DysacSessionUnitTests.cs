using DFC.App.MatchSkills.Application.Dysac;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using ILogger = Castle.Core.Logging.ILogger;

namespace DFC.App.MatchSkills.Services.Dysac.Test.Unit
{
    public class DysacSessionUnitTests
    {

        public class CreateNewSessionTests
        {
            private IOptions<DysacSettings> _dysacServiceSetings;
            private IDysacSessionReader _dysacService;
            
            [SetUp]
            public void Init()
            {
                
                _dysacServiceSetings = Options.Create(new DysacSettings());
                _dysacServiceSetings.Value.ApiUrl = "https://dev.api.nationalcareersservice.org.uk/something";
                _dysacServiceSetings.Value.ApiKey = "mykeydoesnotmatterasitwillbemocked";
                _dysacServiceSetings.Value.DysacUrl="http://dysacurl";
                _dysacService = Substitute.For<IDysacSessionReader>();
            }

            [Test]
            public void When_DysacService_ObjectCreated()
            {
                
                var logger = Substitute.For<ILogger<DysacService>>();
                var restClient = Substitute.For<IRestClient>();
                var dysacService = new DysacService(logger,restClient,_dysacServiceSetings);
            }

            [Test]
            public void When_InitiateDysacWithNoErrors_ReturnOK()
            {
                _dysacService.InitiateDysac().ReturnsForAnyArgs(new DysacServiceResponse()
                {
                    ResponseCode = DysacReturnCode.Ok
                });
                
                var results = _dysacService.InitiateDysac().Result;
                results.ResponseCode.Should().Be(DysacReturnCode.Ok);
            }

            [Test]
            public void When_InitiateDysacWithErrors_ReturnErrorAndMessage()
            {
                _dysacService.InitiateDysac().ReturnsForAnyArgs(new DysacServiceResponse()
                {
                    ResponseCode = DysacReturnCode.Error,
                    ResponseMessage = "Error"
                });
                
                var results = _dysacService.InitiateDysac().Result;
                results.ResponseCode.Should().Be(DysacReturnCode.Error);
                results.ResponseMessage.Should().Be("Error");
            }
        }




    }
}
