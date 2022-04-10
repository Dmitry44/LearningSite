using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningSite.Test.Helpers
{
    public class AppDbContextFactory : IDisposable
    {
        private DbConnection? _connection;

        private DbContextOptions<AppDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection!).Options;
        }

        public AppDbContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                var options = CreateOptions();
                using (var context = new AppDbContext(options))
                {
                    context.Database.EnsureCreated();
                    SeedTestData(context);
                }
            }

            return new AppDbContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        private void SeedTestData(AppDbContext db)
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
            db.AppUsers.AddRange(appUsers);

            db.SaveChanges();
        }

    }
}
