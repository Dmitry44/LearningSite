using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace LearningSite.Web.Pages
{
    [AllowAnonymous]
    public class TestModel : PageModel
    {
        private readonly AppDbContext db;

        public TestModel(AppDbContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
        }

        public void OnGetClearAllPools()
        {
            SqliteConnection.ClearAllPools();

        }

        public async Task OnGetSeedDb()
        {
            var appUsers = new List<AppUser>
            {
                new AppUser {
                    Name = "Admin",
                    EmailAddress = "a@a.a",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("aaaaaa", "Salt"),
                    IsAdmin = true,
                    IsActive = true,
                    TimeZoneId = "Russian Standard Time"
                },
                new AppUser {
                    Name = "User D",
                    EmailAddress = "d@d.d",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("dddddd", "Salt"),
                    IsAdmin = false,
                    IsActive = true,
                    TimeZoneId = "Eastern Standard Time"
                }
            };
            db.AppUsers.AddRange(appUsers);

            var lessons = new List<Lesson>()
            {
                new Lesson() {
                    Name = "English",
                    IsActive = true,
                    Decription = "Some long description of English laguages",
                    Packages = new List<Package>()
                    {
                        new Package() {
                            Name = "English 1 30",
                            IsActive= true,
                            Minutes = 30,
                            Quantity = 1,
                            PriceStr = "15.00 $",
                            PaymentUrl = "/Index?t=1"
                        },
                        new Package() {
                            Name = "English 1 60",
                            IsActive= true,
                            Minutes = 60,
                            Quantity = 1,
                            PriceStr = "25.00 $",
                            PaymentUrl = "/Index?t=2"
                        },
                        new Package() {
                            Name = "English 10 60",
                            IsActive= true,
                            Minutes = 60,
                            Quantity = 10,
                            PriceStr = "230.00 $",
                            PaymentUrl = "/Index?t=3"
                        }
                    }
                },
                new Lesson() {
                    Name = "Spanish",
                    IsActive = true,
                    Decription = "Some long description of Spanish laguages",
                    Packages = new List<Package>()
                    {
                        new Package() {
                            Name = "Spanish 1 30",
                            IsActive= true,
                            Minutes = 30,
                            Quantity = 1,
                            PriceStr = "14.00 $",
                            PaymentUrl = "/Index?t=4"
                        },
                        new Package() {
                            Name = "Spanish 1 60",
                            IsActive= true,
                            Minutes = 60,
                            Quantity = 1,
                            PriceStr = "24.00 $",
                            PaymentUrl = "/Index?t=5"
                        },
                        new Package() {
                            Name = "Spanish 10 60",
                            IsActive= true,
                            Minutes = 60,
                            Quantity = 10,
                            PriceStr = "220.00 $",
                            PaymentUrl = "/Index?t=6"
                        }
                    }
                },
                new Lesson() {
                    Name = "Drawing",
                    IsActive = true,
                    Decription = "Some long description of drawing",
                    Packages = new List<Package>()
                    {
                        new Package() {
                            Name = "Drowing 1 60",
                            Decription = "Drawing - description from package!",
                            IsActive= true,
                            Minutes = 60,
                            Quantity = 1,
                            PriceStr = "30.00 $",
                            PaymentUrl = "/Index?t=7"
                        }
                    }
                }
            };
            db.Lessons.AddRange(lessons);

            await db.SaveChangesAsync();
        }

    }
}