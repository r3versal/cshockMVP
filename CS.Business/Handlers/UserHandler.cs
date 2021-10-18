using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Security.Cryptography;
using System.IdentityModel.Tokens;


namespace CS.Business.Handlers
{
    using CS.Common.Models;
    using CS.Common.Utility;
    using System.Collections.Generic;
    using System.Data;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Cryptography;
    using System.Text;


    public class UserHandler
    {

        public static async Task<bool> EmailNotInUse(string email)
        {
            using (var conn = Business.Database.Connection)
            {
                var foundUsers = await conn.QueryAsync<User>("UserGetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);
                if (foundUsers.Count() > 0)
                {
                    return false;
                }
                return true;
            }
        }

        public static async Task<List<User>> UserByEmail(string email)
        {
            using (var conn = Business.Database.Connection)
            {
                var foundUsers = await conn.QueryAsync<User>("UserGetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);
                if (foundUsers.AsList().Count() > 0)
                {
                    return foundUsers.AsList();
                }
                return null;
            }
        }

        public static async Task<List<User>> UpdateUser(User user)
        {
            using (var conn = Business.Database.Connection)
            {
                var foundUsers = await conn.QueryAsync<User>("UserUpdate", user, commandType: CommandType.StoredProcedure);
                if (foundUsers.AsList().Count() > 0)
                {
                    return foundUsers.AsList();
                }
                return null;
            }
        }

        public static async Task<User> GetUserById(Guid userId)
        {
            using (var conn = Business.Database.Connection)
            {
                var foundUsers = await conn.QueryAsync<User>("SELECT * FROM Users WHERE UserId = '" + userId + "'");
                if (foundUsers.AsList().Count() > 0)
                {
                    return foundUsers.AsList()[0];
                }
                return null;
            }
        }


        public static async Task<User> InsertUser(User user, string password)
        {
            SaltHashPair shp = Security.CreateSaltAndHash(password);
            UserDataModel udm = new UserDataModel();
            DateTime d = DateTime.UtcNow;
            udm.UserId = Guid.NewGuid();
            udm.Email = user.Email;
            udm.AdminComment = "Created By Couture Shock API";
            udm.PasswordSalt = shp.Salt;
            udm.PasswordHash = shp.Hash;
            udm.Active = true;
            udm.IsSystemAccount = false;
            udm.SystemName = "CSHOCK API";
            if (user.LastIpAddress != null)
            {
                udm.LastIpAddress = user.LastIpAddress;
            }
            else
            {
                udm.LastIpAddress = "n/a";
            }
            udm.CreatedOnUtc = d;
            udm.CreatedBy = "CS.API";
            udm.UpdatedBy = "CS.API";
            udm.UpdatedOnUtc = d;
            udm.AccessFailedCount = 0;

            using (var conn = Business.Database.Connection)
            {
                var newUsers = await conn.QueryAsync<User>("UserInsert", new
                {
                    udm.UserId,
                    udm.Email,
                    udm.AdminComment,
                    udm.PasswordSalt,
                    udm.PasswordHash,
                    udm.Active,
                    udm.IsSystemAccount,
                    udm.SystemName,
                    udm.LastIpAddress,
                    udm.CreatedOnUtc,
                    udm.CreatedBy,
                    udm.UpdatedBy,
                    udm.UpdatedOnUtc,
                    udm.AccessFailedCount
                },
                    commandType: CommandType.StoredProcedure);

                if (newUsers.Count() > 0)
                {
                    return newUsers.AsList()[0];
                }
                return null;
            }

        }

        public static async Task<User> Authenticate(LoginRequest request)
        {
            using (var conn = Business.Database.Connection)
            {
                string email = request.Email;
                var users = await conn.QueryAsync<User>("UserGetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);
                if (users == null || users.AsList().Count == 0)
                    return null;
                User u = users.AsList()[0];

                string passHash = Security.CreateHash(request.Password, u.PasswordSalt);
                if (u.PasswordHash == passHash)
                {
                    u.PasswordHash = null;
                    u.PasswordSalt = null;
                    return u;
                }
                return null;
            }
        }

        public static async Task<PasswordReset> PasswordResetHash(string email)
        {
            using (var conn = Business.Database.Connection)
            {
                var users = await conn.QueryAsync<User>("UserGetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);
                if (users == null || users.AsList().Count == 0)
                {
                    return null;
                }

                User u = users.AsList()[0];
                Guid passwordResetId = Guid.NewGuid();
                Guid userId = u.UserId;
                Guid forHash = Guid.NewGuid();
                string newHash = ComputeSha256Hash(forHash.ToString());
                DateTime expire = DateTime.Now.AddMinutes(60);

                var deleteOldHash = await CheckPreviousResetHash(userId);

                var pwreset = await conn.QueryAsync<PasswordReset>("CreatePWReset", new
                {
                    passwordResetId,
                    userId,
                    newHash,
                    expire

                }, commandType: CommandType.StoredProcedure);
                return pwreset.AsList()[0];
            }
        }


        public static async Task<PasswordReset> CheckPasswordResetHash(string newHash)
        {
            using (var conn = Business.Database.Connection)
            {
                var pwHash = await conn.QueryAsync<PasswordReset>("CheckPWResetHash", new { newHash }, commandType: CommandType.StoredProcedure);

                return pwHash.AsList()[0];
            }
        }

        public static async Task<bool> CheckPreviousResetHash(Guid UserId)
        {
            using (var conn = Business.Database.Connection)
            {
                var pwHash = await conn.QueryAsync<PasswordReset>("CheckPrevPWResetHash", new { UserId }, commandType: CommandType.StoredProcedure);

                if (pwHash.AsList().Count() > 0)
                {
                    //delete previous hash
                    await conn.ExecuteAsync("DeletePrevPWResetHash", new { UserId }, commandType: CommandType.StoredProcedure);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<User> AuthenticateAdmin(LoginRequest request)
        {
            using (var conn = Business.Database.Connection)
            {
                string email = request.Email;
                var users = await conn.QueryAsync<User>("UserGetByEmail", new { Email = email }, commandType: CommandType.StoredProcedure);
                if (users == null || users.AsList().Count == 0)
                    return null;
                User u = users.AsList()[0];
                if (u.IsSystemAccount == false)
                {
                    return null;
                }
                string passHash = Security.CreateHash(request.Password, u.PasswordSalt);
                if (u.PasswordHash == passHash)
                {
                    u.PasswordHash = null;
                    u.PasswordSalt = null;
                    return u;
                }
                return null;
            }
        }


        public static async Task<User> UpdateUser(User user, string password)
        {
            try
            {
                SaltHashPair shp = Security.CreateSaltAndHash(password);
                UserDataModel udm = new UserDataModel();
                DateTime d = DateTime.UtcNow;
                udm.UserId = user.UserId;
                udm.AdminComment = "Password Updated On: " + d.ToString();
                udm.PasswordSalt = shp.Salt;
                udm.PasswordHash = shp.Hash;
                udm.Active = user.IsActive;
                udm.IsSystemAccount = false;
                udm.SystemName = "CSHOCK API";
                if (user.LastIpAddress != null)
                {
                    udm.LastIpAddress = user.LastIpAddress;
                }
                else
                {
                    udm.LastIpAddress = "n/a";
                }
                udm.UpdatedBy = "CS.API";
                udm.UpdatedOnUtc = d;
                udm.AccessFailedCount = 0;

                using (var conn = Business.Database.Connection)
                {
                    var newUsers = await conn.QueryAsync<User>("UserUpdate", new
                    {
                        udm.UserId,
                        udm.AdminComment,
                        udm.PasswordSalt,
                        udm.PasswordHash,
                        udm.Active,
                        udm.SystemName,
                        udm.LastIpAddress,
                        udm.UpdatedBy,
                        udm.UpdatedOnUtc,
                        udm.AccessFailedCount
                    },
                        commandType: CommandType.StoredProcedure);

                    if (newUsers.Count() > 0)
                    {
                        return newUsers.AsList()[0];
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }




        #region Sha-512
        public static async Task<string> RefreshTokenWithHash(string hashIngredients)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(hashIngredients);
                using (var sha512 = SHA512.Create())
                {
                    // Send a sample text to hash.
                    var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(hashIngredients));

                    // Get the hashed string.
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "");
                    return hash;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region Check Timestamp
        public static async Task<bool> ValidTimestamp(double hashTime)
        {
            try
            {
                System.DateTime deviceTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                deviceTimestamp = deviceTimestamp.AddSeconds(hashTime).ToUniversalTime();

                var currentUTCTime = DateTime.UtcNow;
                var timeWindow = currentUTCTime.AddMinutes(-5);
                var timeDiff = timeWindow.Subtract(deviceTimestamp).TotalMinutes;

                if (timeDiff <= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ValidateToken
        public static bool ReadToken(string token, Guid userId)
        {
            if (token != null)
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                var readToken = tokenHandler.ReadJwtToken(token);

                char splitter = ':';
                string[] substrings = readToken.Subject.Split(splitter);

                if (substrings.Count() == 2)
                {
                    var tokenEmail = substrings[0];
                    var tokenUserId = new Guid(substrings[1]);

                    if (userId == tokenUserId)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        #endregion

    }
}
