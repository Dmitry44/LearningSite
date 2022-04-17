using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LearningSite.Web.Server
{
    [Authorize]
    [RequireHttps]
    public class AppPageModel : PageModel
    {
        protected readonly IMediator mediator;

        public int UserId { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public bool IsAdmin { get; private set; }
        public string UserName { get; private set; } = "";
        public string Email { get; private set; } = "";
        public string TimeZoneId { get; private set; } = "";

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
            IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;
            IsAdmin = IsAuthenticated && User!.IsInRole("Admin");
            if (IsAuthenticated)
            {
                UserName = GetClaimValue(ClaimTypes.Name);
                Email = GetClaimValue(ClaimTypes.Email);
                TimeZoneId = GetClaimValue(ClaimTypes.Locality);
                UserId = int.TryParse(GetClaimValue(ClaimTypes.NameIdentifier), out var intValue) ? intValue : 0;
            }
        }

        private string GetClaimValue(string claim)
        {
            var userNameClaim = User?.Claims.FirstOrDefault(x => x.Type == claim);
            return userNameClaim?.Value ?? "";
        }

        public AppPageModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

    }
}
