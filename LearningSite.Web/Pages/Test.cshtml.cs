using LearningSite.Web.Server;
using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Pages
{
    [AllowAnonymous]
    public class TestModel : PageModel
    {
        private readonly AppDbContext db;
        private readonly SiteSettings siteSettings;

        public TestModel(AppDbContext db, SiteSettings siteSettings)
        {
            this.db = db;
            this.siteSettings = siteSettings;
        }

        public void OnGet()
        {
        }

        public void OnGetClearAllPools()
        {
            SqliteConnection.ClearAllPools();

        }

        public async Task OnGetRecreateSeedDb()
        {
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            await OnGetSeedDb();
        }

        public async Task OnGetSeedDb()
        {
            siteSettings.AvailabilityTimeZoneId = "Europe/London";
            await siteSettings.Save();
            await siteSettings.Load();

            var appUsers = new List<AppUser>
            {
                new AppUser {
                    Name = "Admin",
                    EmailAddress = "a@a.a",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("aaaaaa", "Salt"),
                    IsAdmin = true,
                    IsActive = true,
                    TimeZoneId = "Europe/Moscow"
                },
                new AppUser {
                    Name = "User D",
                    EmailAddress = "d@d.d",
                    Salt = "Salt",
                    PasswordHash = HashHelper.GenerateHash("dddddd", "Salt"),
                    IsAdmin = false,
                    IsActive = true,
                    TimeZoneId = "Europe/Moscow"
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

            var user = appUsers.First(x => x.EmailAddress == "d@d.d" && x.IsActive);
            var package1 = lessons.SelectMany(x => x.Packages)
                .Where(x => x.IsActive && x.Lesson.Name == "English" && x.Name == "English 10 60" && x.IsActive)
                .First();
            var package2 = lessons.SelectMany(x => x.Packages)
                .Where(x => x.IsActive && x.Lesson.Name == "Drawing" && x.IsActive)
                .First();

            var purchases = new List<Purchase>() {
                new Purchase()
                {
                    User = user,
                    Package = package1,
                    IsActive = true,
                    Quantity = package1.Quantity,
                    BookedQuantity = 2,
                    Minutes = package1.Minutes,
                    Name = $"{package1.Lesson.Name}: {package1.Name}",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Purchase()
                {
                    User = user,
                    Package = package1,
                    IsActive = false,
                    Quantity = package1.Quantity,
                    BookedQuantity = package1.Quantity,
                    Minutes = package1.Minutes,
                    Name = $"{package1.Lesson.Name}: {package1.Name}",
                    CreatedAt = DateTime.UtcNow.AddDays(-50)
                },
                new Purchase()
                {
                    User = user,
                    Package = package2,
                    IsActive = true,
                    Quantity = package2.Quantity,
                    BookedQuantity = 0,
                    Minutes = package2.Minutes,
                    Name = $"{package2.Lesson.Name}: {package2.Name}",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };
            db.Purchases.AddRange(purchases);

            var availabilityDefs = new List<AvailabilityDef>()
            {
                new() { DayOfWeek = DayOfWeek.Monday, Start = new(10, 0), End = new(12, 0) },
                new() { DayOfWeek = DayOfWeek.Monday, Start = new(14, 0), End = new(18, 0) },
                new() { DayOfWeek = DayOfWeek.Tuesday, Start = new(11, 0), End = new(13, 0) },
                new() { DayOfWeek = DayOfWeek.Tuesday, Start = new(14, 0), End = new(18, 0) },
                new() { DayOfWeek = DayOfWeek.Wednesday, Start = new(10, 0), End = new(12, 0) },
                new() { DayOfWeek = DayOfWeek.Wednesday, Start = new(14, 0), End = new(18, 0) },
                new() { DayOfWeek = DayOfWeek.Thursday, Start = new(10, 0), End = new(12, 0) },
                new() { DayOfWeek = DayOfWeek.Thursday, Start = new(14, 0), End = new(18, 0) },
                new() { DayOfWeek = DayOfWeek.Friday, Start = new(11, 0), End = new(13, 0) },
                new() { DayOfWeek = DayOfWeek.Friday, Start = new(14, 0), End = new(18, 0) },
                new() { DayOfWeek = DayOfWeek.Saturday, Start = new(11, 0), End = new(15, 0) }
            };
            db.AvailabilityDefs.AddRange(availabilityDefs);

            var customDay1 = DateTime.Today.AddDays(1).Date;
            var customDay2 = DateTime.Today.AddDays(3).Date;
            db.CustomDays.Add(new() { Day = DateOnly.FromDateTime(customDay1) });
            db.CustomDays.Add(new() { Day = DateOnly.FromDateTime(customDay2) });
            db.CustomDays.Add(new() { Day = DateOnly.FromDateTime(DateTime.Today.AddDays(5).Date) });

            var availabilities = new List<Availability>()
            {
                new() { Start = customDay1.AddHours(8), End = customDay1.AddHours(12) },
                new() { Start = customDay1.AddHours(16), End = customDay1.AddHours(19) },
                new() { Start = customDay1.AddHours(7), End = customDay1.AddHours(11) },
                new() { Start = customDay1.AddHours(14), End = customDay1.AddHours(16) },
            };
            db.Availabilities.AddRange(availabilities);

            var bookings = new List<Booking>()
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    User = user,
                    Purchase = purchases.First(),
                    Start = customDay1.AddHours(9),
                    End = customDay1.AddHours(10)
                }
            };
            db.Bookings.AddRange(bookings);

            await db.SaveChangesAsync();
        }

    }
}