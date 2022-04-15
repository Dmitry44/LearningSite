using LearningSite.Web.Server;
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

namespace LearningSite.Test.Server
{
    public class UserManagerTests
    {
        private readonly DefaultHttpContext httpContext;
        private readonly UserManager sut;
        private readonly Mock<IAuthenticationService> authServiceMock;
        private int countSignInAsync;
        private ClaimsPrincipal? claims;

        public UserManagerTests()
        {
            //https://stackoverflow.com/a/47199298/70449
            authServiceMock = new Mock<IAuthenticationService>();
            //authServiceMock
            //    .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(),
            //        It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            //    .Returns(Task.FromResult((object?)null)).Verifiable();
            authServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Callback(callback);

            httpContext = new DefaultHttpContext();
            sut = new UserManager(authServiceMock.Object);
        }

        [Fact]
        public async Task Signin_ShouldReturnStandardClaims_ForUser()
        {
            //arrange
            var vm = new Web.Server.Repositories.AppUserVm()
            {
                UserId = 1,
                Name = "User",
                EmailAddress = "u@u.u",
                TimeZoneId = "Russian Standard Time"
            };

            //act
            await sut.SignIn(httpContext, vm);

            //assert
            Assert.Equal(1, countSignInAsync);
            Assert.NotNull(claims);

            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
            Assert.NotNull(idClaim);
            Assert.Equal("1", idClaim?.Value);

            var nameClaim = claims?.FindFirst(ClaimTypes.Name);
            Assert.NotNull(nameClaim);
            Assert.Equal("User", nameClaim?.Value);

            var emailClaim = claims?.FindFirst(ClaimTypes.Email);
            Assert.NotNull(emailClaim);
            Assert.Equal("u@u.u", emailClaim?.Value);

            var localityClaim = claims?.FindFirst(ClaimTypes.Locality);
            Assert.NotNull(localityClaim);
            Assert.Equal("Russian Standard Time", localityClaim?.Value);
        }

        [Fact]
        public async Task Signin_ShouldReturnStandardAndRoleClaims_ForAdmin()
        {
            //arrange
            var vm = new Web.Server.Repositories.AppUserVm()
            {
                UserId = 2,
                Name = "Admin",
                EmailAddress = "a@a.a",
                IsAdmin = true,
                TimeZoneId = "Russian Standard Time"
            };

            //act
            await sut.SignIn(httpContext, vm);

            //assert
            Assert.Equal(1, countSignInAsync);
            Assert.NotNull(claims);

            var idClaim = claims?.FindFirst(ClaimTypes.NameIdentifier);
            Assert.NotNull(idClaim);
            Assert.Equal("2", idClaim?.Value);

            var nameClaim = claims?.FindFirst(ClaimTypes.Name);
            Assert.NotNull(nameClaim);
            Assert.Equal("Admin", nameClaim?.Value);

            var emailClaim = claims?.FindFirst(ClaimTypes.Email);
            Assert.NotNull(emailClaim);
            Assert.Equal("a@a.a", emailClaim?.Value);

            var roleClaim = claims?.FindFirst(ClaimTypes.Role);
            Assert.NotNull(roleClaim);
            Assert.Equal("Admin", roleClaim?.Value);

            var localityClaim = claims?.FindFirst(ClaimTypes.Locality);
            Assert.NotNull(localityClaim);
            Assert.Equal("Russian Standard Time", localityClaim?.Value);
        }

        private void callback(HttpContext h, string? s, ClaimsPrincipal c, AuthenticationProperties? p)
        {
            countSignInAsync++;
            claims = c;
        }
    }
}
