using LearningSite.Web.Server;
using LearningSite.Web.Server.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages
{
    [AllowAnonymous]
    public class IndexModel : AppPageModel
    {
        private readonly IMediator mediator;

        public IndexModel(IServiceProvider serviceProvider, IMediator mediator) : base(serviceProvider)
        {
            this.mediator = mediator;
        }

        public GetLessons.Vm Lessons { get; private set; } = new();

        public async Task<IActionResult> OnGet()
        {
            Lessons = await mediator.Send(new GetLessons.Query());

            return Page();
        }
    }
}