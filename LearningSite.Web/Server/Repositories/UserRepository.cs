using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using LearningSite.Web.Pages.Account;

namespace LearningSite.Web.Server.Repositories
{
    public interface IUserRepository
    {
        bool IsEmailExists(string email);
        AppUserVm Signup(SignupVm model);
        AppUserVm? Validate(LoginVm model);
    }

    public class UserRepository : IUserRepository
    {
        private AppDbContext db;

        public UserRepository(AppDbContext db)
        {
            this.db = db;
        }

        public bool IsEmailExists(string email)
        {
            return db.AppUsers.Any(x => x.EmailAddress == email);
        }

        public AppUserVm? Validate(LoginVm model)
        {
            var emailRecords = db.AppUsers.Where(x => x.IsActive && x.EmailAddress == model.EmailAddress);

            var results = emailRecords.AsEnumerable()
                .Where(m => m.PasswordHash == HashHelper.GenerateHash(model.Password, m.Salt))
                .Select(m => new AppUserVm()
                {
                    UserId = m.Id,
                    EmailAddress = m.EmailAddress,
                    Name = m.Name,
                    IsAdmin = m.IsAdmin,
                    CreatedAt = m.CreatedAt
                });

            return results.FirstOrDefault();
        }

        public AppUserVm Signup(SignupVm model)
        {
            var salt = HashHelper.GenerateSalt();
            var hashedPassword = HashHelper.GenerateHash(model.Password, salt);

            var user = new AppUser()
            {
                EmailAddress = model.EmailAddress,
                PasswordHash = hashedPassword,
                Salt = salt,
                Name = model.Name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.AppUsers.Add(user);
            db.SaveChanges();

            return new AppUserVm()
            {
                UserId = user.Id,
                EmailAddress = user.EmailAddress,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt
            };
        }
    }

    public class AppUserVm
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
