using LearningSite.Web.Server;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LearningSite.Web.Pages.Schedule
{
    public class IndexModel : AppPageModel
    {
        private readonly IMediator mediator;

        public IndexModel(IMediator mediator) : base(mediator)
        {
            this.mediator = mediator;
        }

        public void OnGet()
        {
        }
    }
}
