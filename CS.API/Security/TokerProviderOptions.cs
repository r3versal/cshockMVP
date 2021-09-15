using System;

namespace CS.API.Security
{
    using Microsoft.IdentityModel.Tokens;

    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(10);

        public TimeSpan RefreshExpiration { get; set; } = TimeSpan.FromMinutes(30);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
