using LearningSite.Web.Server.Entities;
using System.Security.Claims;

namespace LearningSite.Web.Server.Handlers.Auth
{
    public static class UserClaims
    {
        public static List<Claim> GetUserClaims(AppUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));
            claims.Add(new Claim(ClaimTypes.Locality, user.TimeZoneId));
            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            return claims;
        }

    }
}
