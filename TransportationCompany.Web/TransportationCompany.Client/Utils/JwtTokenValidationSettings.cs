using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TransportationCompany.Shared.Config;

namespace TransportationCompany.Client.Utils
{
    public interface IJwtTokenValidationSettings
    {
        string ValidIssuer { get; }
        string ValidAudience { get; }
        string SecretKey { get; }
        TokenValidationParameters CreateTokenValidationParameters();
    }
    public class JwtTokenValidationSettingsFactory : IJwtTokenValidationSettings
    {
        private readonly JwtTokenValidationSettings _settings;
        public string ValidIssuer => _settings.ValidIssuer;

        public string ValidAudience => _settings.ValidAudience;

        public string SecretKey => _settings.SecretKey;

        public TokenValidationParameters CreateTokenValidationParameters()
        {
            var result = new TokenValidationParameters
            {
                ValidIssuer = ValidIssuer,
                ValidAudience = ValidAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey)),

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            return result;
        }

        public JwtTokenValidationSettingsFactory(IOptions<JwtTokenValidationSettings> options)
        {
            _settings = options.Value;
        }
    }
}
