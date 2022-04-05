using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSite.Test.Helpers
{
    //https://stackoverflow.com/a/60497822/70449
    public sealed class TestDbContext : AppDbContext
    {
        public TestDbContext() : base()
        {
            Database.OpenConnection();
            Database.EnsureCreated();
            SeedTestData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=file::memory:?cache=shared");

        public override void Dispose()
        {
            Database.CloseConnection();
            base.Dispose();
        }

        private void SeedTestData()
        {
            var appUsers = new List<AppUser>
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
            };
            AppUsers.AddRange(appUsers);
            
            SaveChanges();
        }

    }
}
