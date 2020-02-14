using DFC.App.MatchSkills.Service;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NUnit.Framework;
using System.Text;

namespace DFC.App.MatchSkills.Test.Service
{
    public class CookieServiceTests
    {
        [Test]
        public void When_AppendCookieCalled_Then_CookieIsUpdated()
        {
            var service = new CookieService(new EphemeralDataProtectionProvider());

            var response = Substitute.For<HttpResponse>();
            var sessionId = "Session";
            service.AppendCookie(sessionId, response);

            

            var protector = (new EphemeralDataProtectionProvider()).CreateProtector(nameof(CookieService));

            var session = protector.Protect(sessionId);

            response.Received().Cookies.Append(CookieService.CookieName, session, Arg.Any<CookieOptions>());
        }

        [Test]
        public void When_CookieFailsToDecrypt_ThenRemoveCookie()
        {
            var service = new CookieService(new EphemeralDataProtectionProvider());

            var response = Substitute.For<HttpResponse>();
            var request = Substitute.For<HttpRequest>();
            var requestCookie = Substitute.For<IRequestCookieCollection>();
            string data = "test";
            requestCookie.TryGetValue(Arg.Any<string>(), out data).ReturnsForAnyArgs(true);
            request.Cookies.ReturnsForAnyArgs(requestCookie);
            service.TryGetPrimaryKey(request, response);

            response.Received().Cookies.Delete(CookieService.CookieName);
        }

        [Test]
        public void When_CookieSuccessfullyDecrypts_ThenReturnPrimaryKey()
        {
            string sessionId = "protectedText";
            var x = Substitute.For<IDataProtector>();
            var f = Substitute.For<IDataProtectionProvider>();
            f.CreateProtector(Arg.Any<string>()).Returns(x);
            x.Unprotect(Arg.Any<byte[]>()).ReturnsForAnyArgs(Encoding.UTF8.GetBytes(sessionId));

            var service = new CookieService(f);

            var response = Substitute.For<HttpResponse>();
            var request = Substitute.For<HttpRequest>();
            var requestCookie = Substitute.For<IRequestCookieCollection>();

            requestCookie.TryGetValue(Arg.Any<string>(), out sessionId).ReturnsForAnyArgs(x=>
            {
                x[1] = "test";
                return true;
            });
            request.Cookies.ReturnsForAnyArgs(requestCookie);
            var result = service.TryGetPrimaryKey(request, response);

            result.Should().Be(sessionId);
        }

        [Test]
        public void When_QueryStringSession_ThenReturnSessionIdFromQueryString()
        {
            string sessionId = "protectedText";
            var service = new CookieService(new EphemeralDataProtectionProvider());

            var response = Substitute.For<HttpResponse>();
            var request = Substitute.For<HttpRequest>();
            request.QueryString = QueryString.Create("sessionId", sessionId);
            var requestCookie = Substitute.For<IRequestCookieCollection>();
            requestCookie.TryGetValue(Arg.Any<string>(), out Arg.Any<string>()).ReturnsForAnyArgs(true);
            request.Cookies.ReturnsForAnyArgs(requestCookie);
            var result = service.TryGetPrimaryKey(request, response);

            result.Should().Be(sessionId);
        }
    }
}
