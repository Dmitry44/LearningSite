using LearningSite.Web.Server;
using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Handlers;
using LearningSite.Web.Server.Handlers.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace LearningSite.Web.Pages.Account
{
    public class IndexModel : AppPageModel
    {
        private readonly TimeZoneProvider timeZoneProvider;

        public IndexModel(TimeZoneProvider timeZoneProvider,
            IMediator mediator) : base(mediator)
        {
            this.timeZoneProvider = timeZoneProvider;
        }

        public SelectListItem[] TimeZoneList
        {
            get => timeZoneProvider.GetTimeZoneList(Vm.TimeZoneId);
        }

        //[BindProperty]
        public UserInfoVm Vm { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var userInfo = await mediator.Send(new GetUserInfo.Request(UserId));
            if (userInfo is null) return NotFound();

            Vm.EmailAddress = userInfo.EmailAddress;
            Vm.Name = userInfo.Name;
            Vm.TimeZoneId = userInfo.TimeZoneId;

            return Page();
        }

        public async Task<IActionResult> OnPost([Bind] UserInfoVm Vm)
        {
            if (!ModelState.IsValid)
                return Page();

            var query = new UpdateUserInfo.Request(new AppUser()
            {
                Id = UserId,
                EmailAddress = Vm.EmailAddress,
                Name = Vm.Name,
                TimeZoneId = Vm.TimeZoneId
            });

            await mediator.Send(query);

            return RedirectToPage("/Account/Index");
        }

        //[BindProperty]
        public ChangePasswordVm ChangePasswordVm { get; set; } = new();

        public async Task<IActionResult> OnPostChangePassword([Bind] ChangePasswordVm ChangePasswordVm)
        {
            if (!ModelState.IsValid)
            {
                return Partial("_ChangePasswordForm", ChangePasswordVm);
            }

            var done = await mediator.Send(
                new ChangePassword.Request(Email, ChangePasswordVm.OldPassword, ChangePasswordVm.NewPassword));

            if (!done)
            {
                ModelState.AddModelError("", "Error! Password was not changed");
                return Partial("_ChangePasswordForm", ChangePasswordVm);
            }

            var resp = Partial("_ChangePasswordForm", new ChangePasswordVm());
            resp.StatusCode = (int)HttpStatusCode.Created;
            return resp;
        }

    }

    public class UserInfoVm
    {
        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = "";

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Required]
        [Display(Name = "Time zone")]
        public string TimeZoneId { get; set; } = "";
    }

    public class ChangePasswordVm
    {
        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; } = "";

        [Required]
        [Display(Name = "Old Password")]
        public string NewPassword { get; set; } = "";
    }

}
