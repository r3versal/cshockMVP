using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CS.Business.Handlers
{
    using Common.Models;
    using System.Data;

    public class AddressHandler
    {
        public static async Task<Address> InsertAddress(Address a)
        {
            using (var conn = Database.Connection)
            {
                var newAddress = await conn.QueryAsync<Address>("AddressInsert", new
                {
                    a.AddressId,
                    a.FirstName,
                    a.LastName,
                    a.Address1,
                    a.Address2,
                    a.City,
                    a.State,
                    a.Country,
                    a.PhoneNumber

                }, commandType: CommandType.StoredProcedure);

                if (newAddress.Count() > 0)
                {
                    return newAddress.AsList()[0];
                }
                return null;
            }
        }

        public static async Task<Address> UpdateAddress(Address a)
        {
            using (var conn = Database.Connection)
            {
                var newAddress = await conn.QueryAsync<Address>("AddressUpdate", new
                {
                    a.AddressId,
                    a.FirstName,
                    a.LastName,
                    a.Address1,
                    a.Address2,
                    a.City,
                    a.State,
                    a.Country,
                    a.PhoneNumber

                }, commandType: CommandType.StoredProcedure);

                if (newAddress.Count() > 0)
                {
                    return newAddress.AsList()[0];
                }
                return null;
            }
        }

        public static async Task<Address> DeleteAddress(Address a)
        {
            using (var conn = Database.Connection)
            {
                var newAddress = await conn.QueryAsync<Address>("AddressDelete", new
                {
                    a.AddressId

                }, commandType: CommandType.StoredProcedure);

                if (newAddress.Count() > 0)
                {
                    return newAddress.AsList()[0];
                }
                return null;
            }
        }
    }
}
