using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Helpers
{
    public class TestDbContextTests
    {
        [Fact]
        public async Task TestDbContext_ShouldBeAcessible()
        {
            using var factory = new AppDbContextFactory();
            using var dbContext = factory.CreateContext();

            Assert.True(await dbContext.Database.CanConnectAsync());
            Assert.True(dbContext.AppUsers.Any());
        }

    }
}
