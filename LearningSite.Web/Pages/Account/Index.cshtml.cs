using LearningSite.Web.Server;
using LearningSite.Web.Server.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Pages.Account
{
    public class IndexModel : AppPageModel
    {
        private readonly TimeZoneProvider timeZoneProvider;
        private readonly AppDbContext db;

        public Dictionary<string, string> Claims { get; private set; } = new();
        public List<TimeZoneProvider.Info> TimeZones { get; private set; }

        public IndexModel(IServiceProvider serviceProvider,
            TimeZoneProvider timeZoneProvider,
            AppDbContext db) : base(serviceProvider)
        {
            TimeZones = timeZoneProvider.TimeZones;
            this.timeZoneProvider = timeZoneProvider;
            this.db = db;
        }

        public SelectListItem[] TimeZoneList
        {
            get => timeZoneProvider.GetTimeZoneList(Vm.TimeZoneId);
        }

        [BindProperty]
        public UserInfoVm? Vm { get; set; }

        public IActionResult OnGet()
        {
            Claims = this.User.Claims.ToDictionary(x => x.Type, x => x.Value);

            Vm = db.AppUsers
                .Where(x => x.Id == UserId)
                .Select(x => new UserInfoVm()
                {
                    EmailAddress = x.EmailAddress,
                    Name = x.Name,
                    TimeZoneId = x.TimeZoneId
                })
                .SingleOrDefault();
            if (Vm is null) return NotFound();

            return Page();
        }
    }
 
    public class UserInfoVm
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
