using LearningSite.Web.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages
{
    [AllowAnonymous]
    public class IndexModel : AppPageModel
    {
        public IndexModel(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}