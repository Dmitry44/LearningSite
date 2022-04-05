using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Server
{
    [Authorize]
    [RequireHttps]
    public class AppPageModel : PageModel
    {
        private readonly IServiceProvider serviceProvider;

        public bool IsAuthenticated { get; private set; }
        public bool IsAdmin { get; private set; }
        public string UserName { get; private set; } = "";

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
            IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            IsAdmin = IsAuthenticated && User!.IsInRole("Admin");
            if (IsAuthenticated)
            {
                var userNameClaim = User?.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Name);
                if (userNameClaim is not null) UserName = userNameClaim.Value;
            }
        }

        public AppPageModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

    }
}
