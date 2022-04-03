using LearningSite.Web.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : AppPageModel
    {
        private readonly IUserManager userManager;

        public LogoutModel(IUserManager userManager, IServiceProvider serviceProvider) : base(serviceProvider)
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
