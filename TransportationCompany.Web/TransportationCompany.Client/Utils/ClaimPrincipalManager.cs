using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using System.Text;
using TransportationCompany.Client.Utils.Config;

namespace TransportationCompany.Client.Utils
{
    public interface IClaimPrincipalManager
    {
        string UserName { get; }
        bool IsAuthenticated { get; }
        ClaimsPrincipal User { get; }
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task RenewTokenAsync(string jwtToken);
        Task<bool> HasPolicy(string policyName);
    }
    public class ClaimPrincipalManager : IClaimPrincipalManager
    {
        private readonly HttpContext httpContext;
        private readonly IAuthorizationService authorizationService;
        private readonly IJwtTokenValidationSettings jwtTokenValidationSettings;
        private readonly IJwtTokenIssuerSettings jwtTokenIssuerSettings;
        private readonly IAuthenticationSettings authenticationSettings;
        public string UserName
        {
            get
            {
                return User.Identities.FirstOrDefault(u => u.IsAuthenticated)?.FindFirst(c => c.Type == "Email").ToString();

            }
        }

        public ClaimsPrincipal User => httpContext?.User;


        public async Task LogoutAsync()
        {
            await httpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
        }

        public ClaimPrincipalManager(IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService,
            IJwtTokenIssuerSettings jwtTokenIssuerSettings,
            IJwtTokenValidationSettings jwtTokenValdationSettings,
            IAuthenticationSettings authenticationSettings)
        {
            httpContext = httpContextAccessor.HttpContext;
            this.authorizationService = authorizationService;
            jwtTokenValidationSettings = jwtTokenValdationSettings;
            this.jwtTokenIssuerSettings = jwtTokenIssuerSettings;
            this.authenticationSettings = authenticationSettings;
        }

        private HttpClient CreateClient()
        {
            var url = new Uri(jwtTokenIssuerSettings.BaseAddress);

            var result = new HttpClient()
            {
                BaseAddress = url
            };

            result.DefaultRequestHeaders.Accept.Clear();
            result.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return result;
        }
        public async Task<string> FetchJwtToken(string email, string password)
        {
            var apiUrl = jwtTokenIssuerSettings.Login;


            using (var client = CreateClient())
            {
                var resource = new
                {
                    UserName = email,
                    Password = password
                };
                using (var content = new StringContent(JsonConvert.SerializeObject(resource), Encoding.UTF8, "application/json"))
                {
                    using (var response = await client.PostAsync(apiUrl, content))
                    {

                        var result = response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : string.Empty;

                        return result;
                    }
                }
            }
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            var jwtToken = await FetchJwtToken(email, password);
            var result = await Login(jwtToken);

            if (result)
            {
                httpContext.Response.Cookies.Append("authname", jwtToken);
            }
            return result;
        }
        private async Task<bool> Login(string jwtToken)
        {
            try
            {
                if (jwtToken.IsNullOrEmpty())
                    return false;

                await LogoutAsync();

                var tokenHandler = new JwtSecurityTokenHandler();

                var settings = jwtTokenValidationSettings.CreateTokenValidationParameters();

                var principal = tokenHandler.ValidateToken(jwtToken, settings, out var validatedToken);

                var identity = principal.Identity as ClaimsIdentity;

                var securityToken = tokenHandler.ReadJwtToken(jwtToken) as JwtSecurityToken;

                var extraClaims = securityToken.Claims.Where(c => !identity.Claims.Any(x => x.Type == c.Type)).ToList();

                extraClaims.Add(new Claim("jwt", jwtToken));

                identity.AddClaims(extraClaims);

                var authenticationProperties = new AuthenticationProperties()
                {
                    IssuedUtc = identity.Claims.First(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value.ToInt64().ToUnixEpochDate(),
                    ExpiresUtc = identity.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value.ToInt64().ToUnixEpochDate(),
                    IsPersistent = true

                };
                await httpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, principal, authenticationProperties);

                return identity.IsAuthenticated;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
        public async Task RenewTokenAsync(string jwtToken)
        {
            var apiUrl = jwtTokenIssuerSettings.RenewToken;

            using (var httpClient = CreateClient())
            {
                using (var content = new FormUrlEncodedContent(new Dictionary<string, string>()))
                {
                    using (var response = await httpClient.PostAsJsonAsync(apiUrl, content))
                    {
                        var renewdToken = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == HttpStatusCode.OK)
                            await Login(renewdToken);
                    }
                }
            }
        }
        public async Task<bool> HasPolicy(string policyName)
        {
            var result = await authorizationService.AuthorizeAsync(User, null, policyName);

            return result.Succeeded;
        }
        public bool IsAuthenticated
        {
            get
            {
                return User.Identities.Any(identity => identity.IsAuthenticated);
            }
        }


    }
}