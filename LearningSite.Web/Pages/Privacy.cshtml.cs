using LearningSite.Web.Server;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LearningSite.Web.Pages
{
    [AllowAnonymous]
    public class PrivacyModel : AppPageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger, IMediator mediator) : base(mediator)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}