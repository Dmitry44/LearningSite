using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Handlers.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Handlers.Auth
{
    public class SignOutTests
    {
        [Fact]
        public async Task SignOut_ShoulCallAuthService()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var httpContext = Mock.Of<HttpContext>();
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Verifiable();
            var sut = new SignOut.Handler(authServiceMock.Object);

            //act
            await sut.Handle(new SignOut.Request(httpContext), new System.Threading.CancellationToken()) ;

            //assert
            authServiceMock.Verify();
        }

    }
}
