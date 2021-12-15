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

        public static async Task<Profile> GetProfile(Guid userId)
        {
            if (userId != null)
            {
                //insert profile
                using (var conn = Business.Database.Connection)
                {
                    string queryString = "SELECT * FROM CustomerProfile WHERE userId = '" + userId + "'";
                    var newProfile = await conn.QueryAsync<CustomerProfile>(queryString);
                    if (newProfile.Count() > 0)
                    {
                        //get measurements
                        var measurements = await conn.QueryAsync<Measurements>("SELECT * FROM Measurements WHERE userId = '" + userId + "'");
                        var customerProfile = newProfile.AsList()[0];
                        Profile returnedProfile = new Profile();
                        returnedProfile.customerProfile = customerProfile;
                        if(measurements.AsList().Count > 0)
                        {
                            returnedProfile.profileMeasurements = measurements.AsList()[0];
                        }

                        //get order history
                        var orders = await conn.QueryAsync<Order>("SELECT * FROM orders WHERE userId = '" + userId + "'");
                        if (orders.AsList().Count > 0)
                        {
                            returnedProfile.orderHistory = orders.AsList();
                        }
                        return returnedProfile;
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<CustomerProfile> InsertProfile(CustomerProfile profile)
        {
            if (profile != null)
            {
                //insert profile
                using (var conn = Business.Database.Connection)
                {
                    profile.CustomerProfileId = Guid.NewGuid();
                    profile.createdOn = DateTime.UtcNow;
                    profile.updatedOn = DateTime.UtcNow;
                    var newProfile = await conn.QueryAsync<CustomerProfile>("ProfileInsert", new
                    {
                        profile.CustomerProfileId,
                        profile.UserId,
                        profile.Email,
                        profile.FirstName,
                        profile.LastName,
                        profile.Birthdate,
                        profile.Gender,
                        profile.InstagramHandle,
                        profile.createdOn,
                        profile.updatedOn
                    },
                     commandType: CommandType.StoredProcedure);
                    if (newProfile.Count() > 0)
                    {
                        return newProfile.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }


        public static async Task<CustomerProfile> UpdateProfile(CustomerProfile profile)
        {
           
            if (profile != null)
            {
                //insert profile
                using (var conn = Business.Database.Connection)
                {
                    profile.updatedOn = DateTime.UtcNow;
                    var updatedProfile = await conn.QueryAsync<CustomerProfile>("ProfileUpdate", new
                    {
                        profile.CustomerProfileId,
                        profile.UserId,
                        profile.Email,
                        profile.FirstName,
                        profile.LastName,
                        profile.Birthdate,
                        profile.Gender,
                        profile.InstagramHandle,
                        profile.updatedOn
                    },
                     commandType: CommandType.StoredProcedure);

                    if (updatedProfile.Count() > 0)
                    {
                        return updatedProfile.AsList()[0];
                    }
                    return null;
                }
            }
            return null;

        }

    }
}
