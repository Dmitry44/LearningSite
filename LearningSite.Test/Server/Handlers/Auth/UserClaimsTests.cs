using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Handlers.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Handlers.Auth
{
    public class UserClaimsTests
    {
        [Fact]
        public void UserClaims_ShouldReturnStandardClaims_ForUser()
        {
            //arrange
            var vm = new AppUser()
            {
                Id = 1,
                Name = "User",
                EmailAddress = "u@u.u",
                TimeZoneId = "Europe/Moscow"
            };

            //act
            var claims = UserClaims.GetList(vm);

            //assert
            Assert.NotNull(claims);
            Assert.Equal(4, claims.Count);

            var idClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Assert.NotNull(idClaim);
            Assert.Equal("1", idClaim?.Value);

            var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            Assert.NotNull(nameClaim);
            Assert.Equal("User", nameClaim?.Value);

            var emailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            Assert.NotNull(emailClaim);
            Assert.Equal("u@u.u", emailClaim?.Value);

            var localityClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Locality);
            Assert.NotNull(localityClaim);
            Assert.Equal("Europe/Moscow", localityClaim?.Value);
        }

        [Fact]
        public void UserClaims_ShouldReturnStandardAndRoleClaims_ForAdmin()
        {
            //arrange
            var vm = new AppUser()
            {
                Id = 2,
                Name = "Admin",
                EmailAddress = "a@a.a",
                IsAdmin = true,
                TimeZoneId = "Europe/Moscow"
            };

            //act
            var claims = UserClaims.GetList(vm);

            //assert
            Assert.NotNull(claims);
            Assert.Equal(5, claims.Count);

            var idClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Assert.NotNull(idClaim);
            Assert.Equal("2", idClaim?.Value);

            var nameClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            Assert.NotNull(nameClaim);
            Assert.Equal("Admin", nameClaim?.Value);

            var emailClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            Assert.NotNull(emailClaim);
            Assert.Equal("a@a.a", emailClaim?.Value);

            var roleClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            Assert.NotNull(roleClaim);
            Assert.Equal("Admin", roleClaim?.Value);

            var localityClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.Locality);
            Assert.NotNull(localityClaim);
            Assert.Equal("Europe/Moscow", localityClaim?.Value);
        }
    }
}
