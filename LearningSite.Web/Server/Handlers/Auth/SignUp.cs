using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LearningSite.Web.Server.Handlers.Auth
{
    public class SignUp
    {
        public record Request(HttpContext HttpContext,
            string EmailAddress,
            string Name,
            string TimeZoneId,
            string Password) : IRequest;

        public class Handler : IRequestHandler<Request>
        {
            private readonly AppDbContext db;
            private readonly IAuthenticationService authenticationService;

            public Handler(AppDbContext db, IAuthenticationService authenticationService)
            {
                this.db = db;
                this.authenticationService = authenticationService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

                var salt = HashHelper.GenerateSalt();
                var hashedPassword = HashHelper.GenerateHash(request.Password, salt);

                AppUser user = new ()
                {
                    EmailAddress = request.EmailAddress,
                    PasswordHash = hashedPassword,
                    Salt = salt,
                    Name = request.Name,
                    TimeZoneId = request.TimeZoneId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                db.AppUsers.Add(user);
                await db.SaveChangesAsync(cancellationToken);

                string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                // Generate Claims from DbEntity
                var claims = UserClaims.GetUserClaims(user);

                // Add Additional Claims from the Context
                // which might be useful
                // claims.Add(httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name));

                ClaimsIdentity claimsIdentity = new (claims, authenticationScheme);
                ClaimsPrincipal claimsPrincipal = new (claimsIdentity);

                var authProperties = new AuthenticationProperties()
                {
                    // AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.
                    // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.
                    // IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. Required when setting the 
                    // ExpireTimeSpan option of CookieAuthenticationOptions 
                    // set with AddCookie. Also required when setting 
                    // ExpiresUtc.
                    // IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.
                    // RedirectUri = "~/Account/Index"
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await authenticationService.SignInAsync(request.HttpContext, authenticationScheme, claimsPrincipal, authProperties);

                await transaction.CommitAsync(cancellationToken);

                return Unit.Value;
            }

        }

    }
}
