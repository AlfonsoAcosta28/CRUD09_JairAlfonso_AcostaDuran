using Microsoft.Extensions.Options;

namespace TransportationCompany.Client.Utils.Config
{
    public class JwtTokenIssuerSettings
    {
        public String BaseAddress { get; set; }
        public String Login { get; set; }
        public String RenewToken { get; set; }
    }

    public interface IJwtTokenIssuerSettings
    {
        String BaseAddress { get; }
        String Login { get; }
        String RenewToken { get; }
    }

    public class JwtTokenIssuerSettingsFactory : IJwtTokenIssuerSettings
    {
        private readonly JwtTokenIssuerSettings _settings;
        public String BaseAddress => _settings.BaseAddress;

        public String Login => _settings.Login;

        public String RenewToken => _settings.RenewToken;


        public JwtTokenIssuerSettingsFactory(IOptions<JwtTokenIssuerSettings> settings)
        {
            _settings = settings.Value;
        }
    }
}
