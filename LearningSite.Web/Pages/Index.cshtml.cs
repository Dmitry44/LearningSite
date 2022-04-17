using LearningSite.Web.Server;
using LearningSite.Web.Server.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.Web.Pages
{
    [AllowAnonymous]
    public class IndexModel : AppPageModel
    {

        public IndexModel(IMediator mediator) : base(mediator)
        {
        }

        public GetLessons.Response Lessons { get; private set; } = new(new());

        public async Task<IActionResult> OnGet()
        {
            Lessons = await mediator.Send(new GetLessons.Request());

            return Page();
        }
    }
}