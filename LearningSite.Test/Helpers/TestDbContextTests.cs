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
            using (var dbContext = new TestDbContext())
            {
                Assert.True(await dbContext.Database.CanConnectAsync());
                Assert.True(dbContext.AppUsers.Any());
            }
        }
    }
}
