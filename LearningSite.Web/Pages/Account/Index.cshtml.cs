using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Account
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public Dictionary<string, string> Claims { get; private set; }

        public void OnGet()
        {
            Claims = this.User.Claims.ToDictionary(x => x.Type, x => x.Value);
        }
    }
}
