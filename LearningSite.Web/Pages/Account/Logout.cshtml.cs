using LearningSite.Web.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : AppPageModel
    {
        private readonly IUserManager userManager;

        public LogoutModel(IUserManager userManager, IMediator mediator) : base(mediator)
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
