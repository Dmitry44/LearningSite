using LearningSite.Web.Server;
using LearningSite.Web.Server.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class SignupModel : AppPageModel
    {
        private readonly IUserManager userManager;
        private readonly IUserRepository userRepository;
        private readonly TimeZoneProvider timeZoneProvider;

        public SignupModel(
            IUserManager userManager,
            IUserRepository userRepository,
            TimeZoneProvider timeZoneProvider,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.timeZoneProvider = timeZoneProvider;
        }

        public SelectListItem[] TimeZoneList { get => timeZoneProvider.GetTimeZoneList(Vm.TimeZoneId); }

        public Dictionary<string, string> TimeZoneMap
        {
            get => timeZoneProvider.TimeZones.ToDictionary(x => x.IanaId, x => x.SystemId);
        }

        public IActionResult OnGet()
        {

            return Page();
        }

        [BindProperty]
        public SignupVm Vm { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (userRepository.IsEmailExists(Vm.EmailAddress))
            {
                ModelState.AddModelError("Vm.EmailAddress", "Email is already in use");
                return Page();
            }

            var user = userRepository.Signup(Vm);

            await userManager.SignIn(this.HttpContext, user);

            return RedirectToPage("/Index");
        }

    }

    public class SignupVm
    {
        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = "";

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Required]
        [Display(Name = "Time zone")]
        public string TimeZoneId { get; set; } = "";
    }

}
