using LearningSite.Web.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LearningSite.Web.Pages.Settings
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : AppPageModel
    {
        public IndexModel(IMediator mediator) : base(mediator)
        {
        }

        public void OnGet()
        {
        }
    }
}
