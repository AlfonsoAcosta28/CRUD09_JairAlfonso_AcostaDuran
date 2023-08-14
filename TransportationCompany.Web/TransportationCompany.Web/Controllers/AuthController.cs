using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TransportationCompany.Core.Entities;
using TransportationCompany.DataAccess;
using TransportationCompany.Shared.Config;
using TransportationCompany.Web.Auth;

namespace TransportationCompany.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtIssuerOptions _jwtIssuerOptions;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IOptions<JwtTokenValidationSettings> _jwtConfig;
        public AuthController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IJwtIssuerOptions jwtIssuerOptions,
            RoleManager<IdentityRole> roleManager,
            IOptions<JwtTokenValidationSettings> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtIssuerOptions = jwtIssuerOptions;
            _roleManager = roleManager;
            _jwtConfig = jwtConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }

            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            var resuslt = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (user == null || !(await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
            {
                return Unauthorized();
            }

            var tokenString = await CreateJwtTokenAsync(user);
            var result = new ContentResult()
            {
                Content = tokenString,
                ContentType = "application/text",
            };

            return result;
        }

        private async Task<String> CreateJwtTokenAsync(IdentityUser user)
        {
            var claims = new List<Claim>(new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _jwtIssuerOptions.Issuer),

                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),

                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, _jwtIssuerOptions.IssuedAt.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64)
            });

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            var rolesNames = await _userManager.GetRolesAsync(user);

            foreach (var rolesName in rolesNames)
            {
                var role = await _roleManager.FindByNameAsync(rolesName);
                if (role != null)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, role.Name, ClaimValueTypes.String);
                    claims.Add(roleClaim);

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(claims);
                }
            }

            var jwt = new JwtSecurityToken
            (
                issuer: _jwtIssuerOptions.Issuer,
                audience: _jwtIssuerOptions.Audience,
                claims: claims,
                notBefore: _jwtIssuerOptions.NotBefore,
                expires: _jwtIssuerOptions.Expires,
                signingCredentials: _jwtIssuerOptions.SigningCredentials

            );

            var result = new JwtSecurityTokenHandler().WriteToken(jwt);

            return result;
        }
    }
}
