using LearningSite.Web.Server;
using LearningSite.Web.Server.Handlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : AppPageModel
    {

        public LoginModel(IMediator mediator) : base(mediator)
        {
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public LoginVm Vm { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!await mediator.Send<bool>(new SignIn.Request(this.HttpContext, Vm.EmailAddress, Vm.Password)))
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return Page();
            }

            return LocalRedirect("/Index");
        }

    }

    public class LoginVm
    {
        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = "";

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";
    }

}
