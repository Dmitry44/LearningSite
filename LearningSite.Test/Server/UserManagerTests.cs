using LearningSite.Web.Server;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server
{
    public class UserManagerTests
    {
        private readonly IUserManager sut;
        private readonly Mock<HttpContext> httpContextMock = new();

        public UserManagerTests()
        {
            sut = new UserManager();
        }

        [Fact(Skip = "Not ready")]
        public async Task Signin1()
        {
            //arrange
            //var httpContext = new DefaultHttpContext();
            var vm = new Web.Server.Repositories.AppUserVm();
            //httpContextMock.Setups(x => x.SignIn(It.IsAny<string>())).Returns(Task.FromResult(vm));

            //act
            await sut.SignIn(httpContextMock.Object, vm);

            //assert
            //httpContextMock.Verify(x => x.SignInAsync(It.IsAny<>))
        }

    }
}
