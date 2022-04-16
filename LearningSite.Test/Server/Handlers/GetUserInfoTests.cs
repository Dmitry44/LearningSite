using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Handlers
{
    public class GetUserInfoTests
    {
        [Fact]
        public async Task GetUserInfo_ShoulReturnUserInfo_WhenUserExists()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new GetUserInfo.Handler(db);
            var user = db.AppUsers.First();

            //act
            var rez = await sut.Handle(new GetUserInfo.Query() { UserId = user.Id }, new System.Threading.CancellationToken());

            //assert
            Assert.NotNull(rez);
            Assert.IsType<GetUserInfo.Vm>(rez);
            Assert.Equal(user.EmailAddress, rez.EmailAddress);
            Assert.Equal(user.Name, rez.Name);
            Assert.Equal(user.TimeZoneId, rez.TimeZoneId);
        }

    }
}
