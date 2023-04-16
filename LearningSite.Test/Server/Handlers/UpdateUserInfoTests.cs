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
    public class UpdateUserInfoTests
    {
        [Fact]
        public async Task UpdateUserInfo_ShoulReturnUpdatedUser_WhenUserExists()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new UpdateUserInfo.Handler(db);
            var user = db.AppUsers.First();
            var userAfter = new AppUser()
            {
                Id = user.Id,
                EmailAddress = $"!{user.EmailAddress}",
                Name = $"!{user.Name}",
                TimeZoneId = $"!{user.TimeZoneId}"
            };

            //act
            await sut.Handle(new UpdateUserInfo.Request(userAfter), new System.Threading.CancellationToken());
            var rezDb = await db.AppUsers.FirstAsync(x => x.Id == user.Id);

            //assert
            Assert.NotNull(rezDb);
            Assert.IsType<AppUser>(rezDb);
            Assert.Equal(rezDb.EmailAddress, userAfter.EmailAddress);
            Assert.Equal(rezDb.Name, userAfter.Name);
            Assert.Equal(rezDb.TimeZoneId, userAfter.TimeZoneId);
        }

    }
}
