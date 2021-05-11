using System;

namespace CS.API.Security
{
    public class JwtToken
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
    }
}
