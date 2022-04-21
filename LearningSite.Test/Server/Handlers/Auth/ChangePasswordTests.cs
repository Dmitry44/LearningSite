using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Handlers.Auth;
using LearningSite.Web.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Handlers.Auth
{
    public class ChangePasswordTests
    {
        [Theory]
        [InlineData("a@a.a", "123", "444", true)]
        [InlineData("a@a.a", "333", "444", false)]
        public async Task ChangePassword_ShouldUpdateAppUsers_WhenOldPasswordCorrect(
            string email,
            string oldPassword,
            string newPassword,
            bool expected)
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var oldUser = db.AppUsers.Where(x => x.EmailAddress == email)
                .Select(x => new { x.Salt, x.PasswordHash })
                .First();
            var sut = new ChangePassword.Handler(db);

            //act
            await sut.Handle(new ChangePassword.Request(email, oldPassword, newPassword), new System.Threading.CancellationToken());
            var newSalt = db.AppUsers.First(x => x.EmailAddress == email).Salt;

            //assert
            if (expected)
            {
                Assert.True(db.AppUsers.Any(x => x.EmailAddress == email
                    && x.PasswordHash != oldUser.PasswordHash
                    && x.PasswordHash == HashHelper.GenerateHash(newPassword, newSalt)));
            }
            else
            {
                Assert.True(db.AppUsers.Any(x => x.EmailAddress == email
                    && x.Salt == oldUser.Salt
                    && x.PasswordHash == oldUser.PasswordHash));
            }
        }

    }
}
