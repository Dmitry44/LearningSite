using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Handlers.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Handlers.Auth
{
    public class SignInTests
    {
        [Theory]
        [InlineData("a@a.a", "123", true)] //active
        [InlineData("i@i.i", "345", false)] //inactive
        [InlineData("a@a.a", "1234", false)] //wrong password
        [InlineData("aaaaa@a.a", "123", false)] //email doesn't exist
        public async Task SignIn_ShouldCallAuthService(string email, string password, bool shouldPass)
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var httpContext = Mock.Of<HttpContext>();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()));
            var sut = new SignIn.Handler(db, authServiceMock.Object);

            //act
            await sut.Handle(new SignIn.Request(httpContext, email, password), new System.Threading.CancellationToken());

            //assert
            authServiceMock.Verify(x =>
                x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()),
                shouldPass ? Times.Once : Times.Never);
        }

    }
}
