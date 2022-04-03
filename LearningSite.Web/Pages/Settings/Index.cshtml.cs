using LearningSite.Web.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Settings
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : AppPageModel
    {
        public IndexModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public void OnGet()
        {
        }
    }
}
