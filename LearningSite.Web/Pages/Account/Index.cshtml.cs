using LearningSite.Web.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Account
{
    public class IndexModel : AppPageModel
    {
        public Dictionary<string, string> Claims { get; private set; } = new();
        public List<TimeZoneProvider.Info> TimeZones { get; private set; }

        public IndexModel(IServiceProvider serviceProvider, TimeZoneProvider timeZoneProvider) : base(serviceProvider)
        {
            TimeZones = timeZoneProvider.TimeZones;
        }

        public IActionResult OnGet()
        {
            Claims = this.User.Claims.ToDictionary(x => x.Type, x => x.Value);

            return Page();
        }
    }
}
