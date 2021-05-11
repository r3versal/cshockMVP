using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Common.Utility
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System.Security.Cryptography;

    public class SaltHashPair
    {
        public string Salt { get; set; }
        public string Hash { get; set; }
    }

    public class Security
    {
        public static SaltHashPair CreateSaltAndHash(string password)
        {
            SaltHashPair shp = new SaltHashPair();

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            shp.Salt = Convert.ToBase64String(salt);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            shp.Hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return shp;
        }

        public static string CreateHash(string password, string salt)
        {

            byte[] saltBytes = Convert.FromBase64String(salt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash;
        }
    }
}
