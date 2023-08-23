using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;

namespace TransportationCompany.Client.Utils
{
    public class RefreshTokenMonitor
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var issuedClaim = context.Principal.FindFirst(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
            var issuedAt = issuedClaim.ToInt64().ToUnixEpochDate();

            var expiresClaim = context.Principal.FindFirst(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            var expiresAt = expiresClaim.ToInt64().ToUnixEpochDate();

            var validWindow = (expiresAt - issuedAt).TotalMinutes;

            var refreshDateTime = issuedAt.AddMinutes(0.5 * validWindow);

            if (DateTime.UtcNow > refreshDateTime)
            {
                var jwtToken = context.Principal.FindFirst("jwt").Value;

                var claimPrincipalManger = context.HttpContext.RequestServices.GetService<IClaimPrincipalManager>();

                await claimPrincipalManger.RenewTokenAsync(jwtToken);
            }
        }
    }
}
