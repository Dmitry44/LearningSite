using LearningSite.Web.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IUserManager userManager;

        public LogoutModel(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await userManager.SignOut(this.HttpContext);
            return LocalRedirect("/Index");
        }
    }
}
