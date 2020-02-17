using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.MatchSkills.Application.Session.Interfaces;
using DFC.App.MatchSkills.Application.Session.Models;
using DFC.App.MatchSkills.Controllers;
using DFC.App.MatchSkills.Service;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using NSubstitute;
using FluentAssertions.Common;
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

        public static UserSession GetUserSession(bool withOccupations=true)
        {
            var userSession = new UserSession();
            if (withOccupations)
            {
                userSession.Occupations = new HashSet<UsOccupation>(2)
                    {
                        new UsOccupation("1", "FirstOccupation", DateTime.UtcNow),
                        new UsOccupation("2", "SecondOccupation", DateTime.UtcNow)
                    }
                    ;
            }
            else
            {
                userSession.Occupations = null;
            }
            
            
            return userSession;
        }

        

    }

    
}
