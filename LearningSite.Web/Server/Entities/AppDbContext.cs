using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers => Set<AppUser>();

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
