using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.API.Security
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    
    using Newtonsoft.Json;

    using CS.API.Security;
    using CS.Common.Models;
    

    public class JwtSecurity
    {
        // secretKey contains a secret passphrase only your server knows
        public static string secretKey;
        public static SymmetricSecurityKey signingKey;

        private static IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            set {
                _configuration = value;
                secretKey = _configuration["JwtSecurity:SecretKey"];
                signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            }
            get
            {
                return _configuration;
            }
        }


        public static JwtToken GenerateToken(Guid userId, string username)
        {
            var options = new TokenProviderOptions
            {
                Audience = "User",
                Issuer = "CSAPI",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };

            var now = DateTime.UtcNow;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan span = (now.ToLocalTime() - epoch);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username + ":" + userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, span.TotalSeconds.ToString(), ClaimValueTypes.Integer64)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(options.Expiration),
                signingCredentials: options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new JwtToken
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)options.Expiration.TotalSeconds
            };

            return response;
        }
        public static User GetCurrentUser(HttpContext context)
        {
            var userIds = context.User.FindFirst(ClaimTypes.NameIdentifier).Value.Split(':');

            if (userIds.Length < 2)
                return null;

            User u = new User()
            {
                UserId = Guid.Parse(userIds[1]),
                Email = userIds[0]
            };

            return u;
        }
    }
}
