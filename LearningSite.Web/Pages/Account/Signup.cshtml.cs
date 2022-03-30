using LearningSite.Web.Server;
using LearningSite.Web.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    public class SignupModel : PageModel
    {
        private readonly IUserManager userManager;
        private readonly IUserRepository userRepository;

        public SignupModel(IUserManager userManager, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }


        public void OnGet()
        {

        }

        [BindProperty]
        public SignupVm Vm { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = userRepository.Signup(Vm);

            await userManager.SignIn(this.HttpContext, user, false);

            return RedirectToPage("/Index");
        }

    }

    public class SignupVm
    {
        [Required, EmailAddress]
        public string EmailAddress { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }

}
