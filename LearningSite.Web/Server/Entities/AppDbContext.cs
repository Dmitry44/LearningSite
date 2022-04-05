using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Entities
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<AppUser> AppUsers => Set<AppUser>();
        public virtual DbSet<Article> Articles => Set<Article>();
        public virtual DbSet<Availability> Availabilities => Set<Availability>();
        public virtual DbSet<AvailabilityDef> AvailabilityDefs => Set<AvailabilityDef>();
        public virtual DbSet<Booking> Bookings => Set<Booking>();
        public virtual DbSet<Lesson> Lessons => Set<Lesson>();
        public virtual DbSet<Package> Packages => Set<Package>();
        public virtual DbSet<Purchase> Purchases => Set<Purchase>();

        public string DbPath { get; } = "Database/learning-site.db";

        public AppDbContext() { }

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
