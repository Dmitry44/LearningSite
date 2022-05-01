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
    public class SetSettingTests
    {
        [Fact]
        public async Task SetSetting_ShoulInsertValue_WhenNoKey()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new SetSetting.Handler(db);
            var before = await db.Settings.FirstOrDefaultAsync(x => x.Key == "key");

            //act
            await sut.Handle(new SetSetting.Request("key", "value"), new System.Threading.CancellationToken());

            //assert
            var after = await db.Settings.FirstOrDefaultAsync(x => x.Key == "key");
            Assert.Null(before);
            Assert.NotNull(after);
            Assert.IsType<Setting>(after);
            Assert.Equal("key", after?.Key);
            Assert.Equal("value", after?.Value);
        }

        [Fact]
        public async Task SetSetting_ShoulUpdateValue_WhenKeyExists()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new SetSetting.Handler(db);
            db.Settings.Add(new() { Key = "key", Value = "value" });
            await db.SaveChangesAsync();

            //act
            await sut.Handle(new SetSetting.Request("key", "value2"), new System.Threading.CancellationToken());

            //assert
            var after = await db.Settings.FirstOrDefaultAsync(x => x.Key == "key");
            Assert.NotNull(after);
            Assert.IsType<Setting>(after);
            Assert.Equal("key", after?.Key);
            Assert.Equal("value2", after?.Value);
        }

    }
}
