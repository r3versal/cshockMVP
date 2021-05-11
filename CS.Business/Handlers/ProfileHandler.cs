using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using CS.Common.Models;

namespace CS.Business.Handlers
{
    public class ProfileHandler
    {
        public static async Task<User> InsertProfile(Profile profile, User user)
        {
            using (var conn = Database.Connection)
            {
                var p = new DynamicParameters();
                p.Add("@ProfileId", Guid.NewGuid());
                p.Add("@UserId", user.UserId);
                if(profile.FirstName != null)
                {
                    p.Add("@FirstName", profile.FirstName);
                }
                if (profile.LastName != null)
                {
                    p.Add("@LastName", profile.LastName);
                }
                if (profile.Birthdate != null)
                {
                    p.Add("@Birthdate", profile.Birthdate);
                }
                if (profile.IsStore != true)
                {
                    p.Add("@IsStore", false);
                }
                else
                {
                    p.Add("@IsStore", true);
                }
                if (profile.Gender != null)
                {
                    p.Add("@Gender", profile.Gender);
                }
                if (profile.TimeZoneId != null)
                {
                    p.Add("@TimeZoneId", profile.TimeZoneId);
                }
                if (profile.InstagramHandle != null)
                {
                    p.Add("@Birthdate", profile.Birthdate);
                }
                conn.Execute("ProfileInsert", p, commandType: CommandType.StoredProcedure);

                return null;

            }

        }

        public static async Task<Profile> UpdateProfile(User user, Profile profile)
        {
            profile.UserId = user.UserId;
            profile.Email = user.Email;

            using (var conn = Business.Database.Connection)
            {
                var updatedProfile = await conn.QueryAsync<Profile>("ProfileUpdate", profile, commandType: CommandType.StoredProcedure);
                if (updatedProfile.Count() > 0)
                {
                    return updatedProfile.AsList()[0];
                }
                return null;
            }
        }

    }
}
