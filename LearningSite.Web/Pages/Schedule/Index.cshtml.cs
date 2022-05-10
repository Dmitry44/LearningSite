using LearningSite.Web.Server;
using LearningSite.Web.Server.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Schedule
{
    public class IndexModel : AppPageModel
    {

        public IndexModel(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IActionResult> OnGet()
        {
            var vv = await mediator.Send(new GetSchedule.Request(UserId, TimeZoneId));

            return Page();
        }
    }
}
