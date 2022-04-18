using LearningSite.Web.Server;
using LearningSite.Web.Server.Handlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : AppPageModel
    {

        public LogoutModel(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await mediator.Send(new SignOut.Request(this.HttpContext));
            return LocalRedirect("/Index");
        }
    }
}
