using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningSite.Web.Server.Handlers.Auth
{
    public class SignOut
    {
        public record Request(HttpContext HttpContext) : IRequest;

        public class Handler : IRequestHandler<Request>
        {
            private readonly IAuthenticationService authenticationService;

            public Handler(IAuthenticationService authenticationService)
            {
                this.authenticationService = authenticationService;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                await authenticationService.SignOutAsync(request.HttpContext,
                    CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties());
            }

        }

    }
}
