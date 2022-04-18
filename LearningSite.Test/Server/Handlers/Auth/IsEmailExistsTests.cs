using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Handlers.Auth;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Handlers.Auth
{
    public class IsEmailExistsTests
    {
        [Theory]
        [InlineData("a@a.a", true)]
        [InlineData("no@such.email", false)]
        public async Task IsEmailExistsTests_ShoulCheckEmail(string email, bool expexted)
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new IsEmailExists.Handler(db);
            var user = db.AppUsers.First();

            //act
            var rez = await sut.Handle(new IsEmailExists.Request(email), new System.Threading.CancellationToken());

            //assert
            Assert.Equal(expexted, rez);
        }

    }
}
