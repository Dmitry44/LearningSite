using LearningSite.Web.Server;
using LearningSite.Web.Server.Handlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    [AllowAnonymous]
    public class SignupModel : AppPageModel
    {
        private readonly TimeZoneProvider timeZoneProvider;

        public SignupModel(
            TimeZoneProvider timeZoneProvider,
            IMediator mediator) : base(mediator)
        {
            this.timeZoneProvider = timeZoneProvider;
        }

        public SelectListItem[] TimeZoneList { get => timeZoneProvider.GetTimeZoneList(Vm.TimeZoneId); }

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

            if (await mediator.Send<bool>(new IsEmailExists.Request(Vm.EmailAddress)))
            {
                ModelState.AddModelError("Vm.EmailAddress", "Email is already in use");
                return Page();
            }

            await mediator.Send(new SignUp.Request(HttpContext, Vm.EmailAddress, Vm.Name, Vm.TimeZoneId, Vm.Password));

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
