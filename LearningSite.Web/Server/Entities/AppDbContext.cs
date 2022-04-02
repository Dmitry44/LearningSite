using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Availability> Availabilities => Set<Availability>();
        public DbSet<AvailabilityDef> AvailabilityDefs => Set<AvailabilityDef>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<Purchase> Purchases => Set<Purchase>();

        public string DbPath { get; } = "Database/learning-site.db";

        public AppDbContext(DbContextOptions options) : base(options)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = System.IO.Path.Join(path, "learning-site.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

    }
}
