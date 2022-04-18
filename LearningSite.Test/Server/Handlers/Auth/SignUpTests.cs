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
    public class SignUpTests
    {
        [Fact]
        public async Task SignUp_ShouldCallAuthServiceAndAddUser()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            (string email, string name, string timeZoneId, string password) data =
                (Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var httpContext = Mock.Of<HttpContext>();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Verifiable();
            var sut = new SignUp.Handler(db, authServiceMock.Object);

            //act
            await sut.Handle(new SignUp.Request(httpContext, data.email, data.name, data.timeZoneId, data.password), new System.Threading.CancellationToken());

            //assert
            authServiceMock.Verify();
            Assert.True(db.AppUsers.Any(x => x.EmailAddress == data.email));
        }

        [Fact]
        public async Task SignUp_ShouldDoNothing_WhenAuthServiceThrowException()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            (string email, string name, string timeZoneId, string password) data =
                (Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var httpContext = Mock.Of<HttpContext>();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .ThrowsAsync(new Exception("Error!"));
            var sut = new SignUp.Handler(db, authServiceMock.Object);

            //act
            var ex = await Record.ExceptionAsync(() =>
                sut.Handle(new SignUp.Request(httpContext, data.email, data.name, data.timeZoneId, data.password),
                new System.Threading.CancellationToken()));

            //assert
            Assert.IsAssignableFrom<Exception>(ex);
            Assert.False(db.AppUsers.Any(x => x.EmailAddress == data.email));
        }
    }
}
