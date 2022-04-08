using LearningSite.Web.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Account
{
    public class IndexModel : AppPageModel
    {
        public Dictionary<string, string> Claims { get; private set; }
        public Dictionary<string, string> TimeZones { get; private set; }

        public IndexModel(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public void OnGet()
        {
            Claims = this.User.Claims.ToDictionary(x => x.Type, x => x.Value);

            System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo>  tzCollection = TimeZoneInfo.GetSystemTimeZones();
            TimeZones = tzCollection.ToDictionary(x => x.Id, x => x.DisplayName);
        }
    }
}
