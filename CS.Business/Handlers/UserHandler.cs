using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CS.Business.Handlers
{
    using CS.Common.Models;
    using CS.Common.Utility;
    using System.Data;
    using System.Diagnostics;

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


        public static async Task<User> InsertUser(User user, string password)
        {
            SaltHashPair shp = Security.CreateSaltAndHash(password);
            UserDataModel udm = new UserDataModel();
            DateTime d = DateTime.UtcNow;
            udm.UserId = Guid.NewGuid();
            udm.Email = user.Email;
            udm.AdminComment = "This is a test account";
            udm.PasswordSalt = shp.Salt;
            udm.PasswordHash = shp.Hash;
            udm.Active = true;
            udm.IsSystemAccount = true;
            udm.SystemName = "Test System";
            udm.LastIpAddress = "100.111.111.12";
            udm.CreatedOnUtc = d;
            udm.CreatedBy = "Admin";
            udm.UpdatedBy = "Admin";
            udm.UpdatedOnUtc = d;
            udm.AccessFailedCount = 0;

            using (var conn = Business.Database.Connection)
            {
                var newUsers = await conn.QueryAsync<User>("UserInsert", new {
                                                            udm.UserId,
                                                            udm.Email,
                                                            udm.AdminComment,
                                                            udm.PasswordSalt,
                                                            udm.PasswordHash,
                                                            IsActive = true,
                                                            IsSystemAccount = false,
                                                            SystemName = "Test System",
                                                            udm.LastIpAddress,
                                                            CreatedOnUtc = d,
                                                            udm.CreatedBy,
                                                            udm.UpdatedBy,
                                                            UpdatedOnUtc = d,
                                                            AccessFailedCount = 0 },
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
    }
}
