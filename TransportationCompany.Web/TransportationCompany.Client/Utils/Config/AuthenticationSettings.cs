using Microsoft.Extensions.Options;

namespace TransportationCompany.Client.Utils.Config
{
    public class AuthenticationSettings
    {
        public String LoginPath { get; set; }
        public String AceessDeniedPath { get; set; }
    }

    public interface IAuthenticationSettings
    {
        PathString LoginPath { get; }
        PathString AccessDeniedPath { get; }
    }

    public class AuthenticationSettingsFactory : IAuthenticationSettings
    {
        public PathString LoginPath { get; private set; }
        public PathString AccessDeniedPath { get; private set; }

        public AuthenticationSettingsFactory(IOptions<AuthenticationSettings> options)
        {
            LoginPath = new PathString(options.Value.LoginPath);
            AccessDeniedPath = new PathString(options.Value.AceessDeniedPath);
        }
    }
}
