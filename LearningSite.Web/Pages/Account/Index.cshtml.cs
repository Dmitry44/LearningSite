using LearningSite.Web.Server;
using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    public class IndexModel : AppPageModel
    {
        private readonly TimeZoneProvider timeZoneProvider;

        public Dictionary<string, string> Claims { get; private set; } = new();
        public List<TimeZoneProvider.Info> TimeZones { get; private set; }

        public IndexModel(TimeZoneProvider timeZoneProvider,
            IMediator mediator) : base(mediator)
        {
            TimeZones = timeZoneProvider.TimeZones;
            this.timeZoneProvider = timeZoneProvider;
        }

        public SelectListItem[] TimeZoneList
        {
            get => timeZoneProvider.GetTimeZoneList(Vm.TimeZoneId);
        }

        [BindProperty]
        public UserInfoVm Vm { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Claims = this.User.Claims.ToDictionary(x => x.Type, x => x.Value);

            var userInfo = await mediator.Send(new GetUserInfo.Request(UserId));
            if (userInfo is null) return NotFound();

            Vm.EmailAddress = userInfo.EmailAddress;
            Vm.Name = userInfo.Name;
            Vm.TimeZoneId = userInfo.TimeZoneId;

            return Page();
        }

        public async Task<IActionResult> OnPost()
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

            var rez = await mediator.Send(query);

            return RedirectToPage("/Account/Index");
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

}
