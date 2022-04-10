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

        public AppDbContext() { }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
