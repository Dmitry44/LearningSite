using LearningSite.Web.Server.Entities;
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
    }
}