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
    public class GetSettingTests
    {
        [Fact]
        public async Task GetSetting_ShoulReturnEmptyString_WhenNoKey()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new GetSetting.Handler(db);

            //act
            var rez = await sut.Handle(new GetSetting.Request("key"), new System.Threading.CancellationToken());

            //assert
            Assert.Equal("", rez);
        }

        [Fact]
        public async Task GetSetting_ShoulReadSetting_WhenKeyExists()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new GetSetting.Handler(db);
            db.Settings.Add(new() { Key = "key", Value = "value" });
            await db.SaveChangesAsync();

            //act
            await sut.Handle(new GetSetting.Request("key"), new System.Threading.CancellationToken());

            //assert
            var after = await db.Settings.FirstOrDefaultAsync(x => x.Key == "key");
            Assert.NotNull(after);
            Assert.IsType<Setting>(after);
            Assert.Equal("key", after?.Key);
            Assert.Equal("value", after?.Value);
        }

    }
}
