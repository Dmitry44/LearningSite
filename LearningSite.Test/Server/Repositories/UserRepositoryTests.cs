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
        private readonly UserRepository sut;

        public UserRepositoryTests()
        {
            var data = new List<AppUser>
            {
                new AppUser {
                    Name = "Admin",
                    EmailAddress = "a@a.a",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("123", "Salt"),
                    IsAdmin = true,
                    IsActive = true
                },
                new AppUser {
                    Name = "User",
                    EmailAddress = "u@u.u",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("234", "Salt"),
                    IsAdmin = false,
                    IsActive = true
                },
                new AppUser {
                    Name = "Inactive",
                    EmailAddress = "i@i.i",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("345", "Salt"),
                    IsAdmin = false,
                    IsActive = false
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<AppUser>>();
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.AppUsers).Returns(mockSet.Object);

            sut = new UserRepository(mockContext.Object);
        }

        [Fact]
        public void Validate_ShouldReturnVm_WhenAdminIsActive()
        {
            //arrange

            //act
            var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "a@a.a", Password = "123" });

            //assert
            Assert.NotNull(appUserVm);
            Assert.Equal("a@a.a", appUserVm?.EmailAddress);
            Assert.True(appUserVm?.IsAdmin);
        }

        [Fact]
        public void Validate_ShouldReturnVm_WhenUserIsActive()
        {
            //arrange

            //act
            var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "u@u.u", Password = "234" });

            //assert
            Assert.NotNull(appUserVm);
            Assert.Equal("u@u.u", appUserVm?.EmailAddress);
            Assert.False(appUserVm?.IsAdmin);
        }

        [Fact]
        public void Validate_ShouldReturnNull_WhenPasswordIncorrect()
        {
            //arrange

            //act
            var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "a@a.a", Password = "!!!" });

            //assert
            Assert.Null(appUserVm);
        }

        [Fact]
        public void Validate_ShouldReturnNull_WhenUserInactive()
        {
            //arrange

            //act
            var appUserVm = sut.Validate(new Web.Pages.Account.LoginVm() { EmailAddress = "i@i.i", Password = "345" });

            //assert
            Assert.Null(appUserVm);
        }
    }
}
