﻿using LearningSite.Web.Server.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace LearningSite.Web.Server
{
    public interface IUserManager
    {
        Task SignIn(HttpContext httpContext, AppUserVm user);
        Task SignOut(HttpContext httpContext);
    }

    public class UserManager : IUserManager
    {
        private readonly IAuthenticationService authenticationService;

        public UserManager(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task SignIn(HttpContext httpContext, AppUserVm user)
        {
            string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            // Generate Claims from DbEntity
            var claims = GetUserClaims(user);

            // Add Additional Claims from the Context
            // which might be useful
            // claims.Add(httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

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

            await authenticationService.SignInAsync(httpContext, authenticationScheme, claimsPrincipal, authProperties);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private List<Claim> GetUserClaims(AppUserVm user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));
            claims.Add(new Claim(ClaimTypes.Locality, user.TimeZoneId));
            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            return claims;
        }
    }
}
