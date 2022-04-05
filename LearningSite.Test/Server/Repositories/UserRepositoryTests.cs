using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using LearningSite.Web.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Repositories
{
    public class UserRepositoryTests
    {

        [Fact]
        public void Validate_ShouldReturnVm_WhenAdminIsActive()
        {
            using (var dbContext = new TestDbContext())
            {
                //arrange
                var sut = new UserRepository(dbContext);

                //act
                var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "a@a.a", Password = "123" });

                //assert
                Assert.NotNull(appUserVm);
                Assert.Equal("a@a.a", appUserVm?.EmailAddress);
                Assert.True(appUserVm?.IsAdmin);
            }
        }

        [Fact]
        public void Validate_ShouldReturnVm_WhenUserIsActive()
        {
            using (var dbContext = new TestDbContext())
            {
                //arrange
                var sut = new UserRepository(dbContext);

                //act
                var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "u@u.u", Password = "234" });

                //assert
                Assert.NotNull(appUserVm);
                Assert.Equal("u@u.u", appUserVm?.EmailAddress);
                Assert.False(appUserVm?.IsAdmin);
            }
        }

        [Fact]
        public void Validate_ShouldReturnNull_WhenPasswordIncorrect()
        {
            using (var dbContext = new TestDbContext())
            {
                //arrange
                var sut = new UserRepository(dbContext);

                //act
                var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "a@a.a", Password = "!!!" });

                //assert
                Assert.Null(appUserVm);
            }
        }

        [Fact]
        public void Validate_ShouldReturnNull_WhenUserInactive()
        {
            using (var dbContext = new TestDbContext())
            {
                //arrange
                var sut = new UserRepository(dbContext);

                //act
                var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "i@i.i", Password = "345" });

                //assert
                Assert.Null(appUserVm);
            }
        }
    }
}
