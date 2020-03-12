using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.LMI.Models;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using Microsoft.AspNetCore.Mvc;

namespace DFC.App.MatchSkills.Test.Helpers
{
    public static class MockHelpers
    {
       
        
    public static  Mock<HttpMessageHandler> GetMockMessageHandler(string contentToReturn="{'Id':1,'Value':'1'}", HttpStatusCode statusToReturn=HttpStatusCode.OK)
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
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusToReturn,
                    Content = new StringContent(contentToReturn)
                })
                .Verifiable();
            return handlerMock;
        }

        public static Mock<HttpContext> SetupControllerHttpContext()
        {
            var httpContext = new Mock<HttpContext>();
            var httpRequest = new Mock<HttpRequest>();
            var httpResponse = new Mock<HttpResponse>();
            var requestCookie = new Mock<IRequestCookieCollection>();
            var _dataProtectionProvider = new EphemeralDataProtectionProvider();
            var _dataProtector = _dataProtectionProvider.CreateProtector(nameof(BaseController));

            string data = _dataProtector.Protect("This is my value");
            requestCookie.Setup(x =>
                x.TryGetValue(It.IsAny<string>(), out data)).Returns(true);

            httpResponse.Setup(x => x.Cookies).Returns(new Mock<IResponseCookies>().Object);
            httpRequest.Setup(x => x.Cookies).Returns(requestCookie.Object);
            httpContext.Setup(x => x.Request).Returns(httpRequest.Object);
            httpContext.Setup(x => x.Response).Returns(httpResponse.Object);
            
            return httpContext;
        }

        public static ControllerContext GetControllerContext()
        {
            var controllerContext  = new ControllerContext{
                HttpContext = new DefaultHttpContext()
            };

            controllerContext.HttpContext = MockHelpers.SetupControllerHttpContext().Object;
            return controllerContext;
        }

        public static UserSession GetUserSession(bool withOccupations=true, bool withMatches = true, bool withSkills = true)
        {
            var userSession = new UserSession();
            if (withOccupations)
            {
                userSession.Occupations = new HashSet<UsOccupation>(2)
                    {
                        new UsOccupation("1", "FirstOccupation"),
                        new UsOccupation("2", "SecondOccupation")
                    }
                    ;
            }
            if (withMatches)
            {
                userSession.OccupationMatches.Add(new OccupationMatch()
                {
                    JobProfileTitle = "Mock Title",
                    JobProfileUri = "http://mockjoburl",
                    LastModified = DateTime.UtcNow,
                    TotalOccupationEssentialSkills = 12,
                    MatchingEssentialSkills = 6,
                    TotalOccupationOptionalSkills = 4,
                    MatchingOptionalSkills = 2,
                    Uri = "MatchUri",
                    JobGrowth = JobGrowth.Increasing
                });
                userSession.OccupationMatches.Add(new OccupationMatch()
                    {
                        JobProfileTitle = "Mock Title2",
                        JobProfileUri = "http://mockjoburl",
                        LastModified = DateTime.UtcNow,
                        TotalOccupationEssentialSkills = 12,
                        MatchingEssentialSkills = 6,
                        TotalOccupationOptionalSkills = 4,
                        MatchingOptionalSkills = 2,
                        Uri = "MatchUri",
                        JobGrowth = JobGrowth.Increasing
                    }
                );
            }

            if (withSkills)
            {
                userSession.Skills.Add(new UsSkill("http://data.europa.eu/esco/skill/ab2bb44a-3956-4028-8715-8b70b1960b99", "lift heavy weights"));
                userSession.Skills.Add(new UsSkill("http://data.europa.eu/esco/skill/28cb374e-6261-4133-8371-f9a5470145da", "operate forklift"));
            }


            return userSession;
        }

        

    }

    
}
