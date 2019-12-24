using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Domain.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy.Test.Unit
{
    [TestFixture]
    class ServiceTaxonomyUnitTests
    {
        [TestCase("https://dev.api.nationalcareersservice.org.uk/GetAllSkills/Execute/","8ed8640b25004e26992beb9164d95139")]
        public async Task When_MockServiceGet_Then_ShouldReturnObject(string url,string apiKey)
        {
            // ARRANGE
            var skillsJson ="{skills:[{\"skillType\": \"competency\",\"skill\": \"collect biological data\",\"alternativeLabels\": [\"biological data analysing\", \"analysing biological records\"],\"uri\": \"aaa\"}]}";
            var handlerMock = GetMockMessageHandler(skillsJson,HttpStatusCode.OK);
            var httpClient = new HttpClient(handlerMock.Object);
            var subjectUnderTest = new ServiceTaxonomyRepository(httpClient);
            
            // ACTs
            var result = await subjectUnderTest.GetAllSkills<Skill[]>(url,apiKey) ;
            
           
            // ASSERT
            result.Should().NotBeNull(); // this is fluent assertions here...
            result[0].Name.Should().Be("collect biological data");

            // also check the 'http' call was like we expected it
            var expectedUri = new Uri(url);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        public  Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn="{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn=HttpStatusCode.OK)
        {
            var handlerMock =  new Mock<HttpMessageHandler>(MockBehavior.Loose);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )

                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = statusToReturn,
                    Content = new StringContent(contentToReturn),
                })
                .Verifiable();
            return handlerMock;
        }

    }
   
}
