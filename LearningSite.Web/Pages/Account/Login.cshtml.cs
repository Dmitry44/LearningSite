using LearningSite.Web.Server;
using LearningSite.Web.Server.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : AppPageModel
    {
        private readonly IUserManager userManager;
        private readonly IUserRepository userRepository;

        public LoginModel(
            IUserManager userManager,
            IUserRepository userRepository,
            IMediator mediator) : base(mediator)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
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

            var user = userRepository.Validate(Vm);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return Page();
            }

            await userManager.SignIn(this.HttpContext, user);

            return LocalRedirect("/Index");
        }

    }

    public class LoginVm
    {
        [Required, EmailAddress]
        public string EmailAddress { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }

}
