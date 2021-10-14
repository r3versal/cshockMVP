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
        public static async Task<CustomerAddress> InsertAddress(CustomerOrderDataModel codm)
        {
            CustomerAddress a = new CustomerAddress();
            a.Address1 = codm.shippingAddress.Address1;
            a.Address2 = codm.shippingAddress.Address2;
            a.City = codm.shippingAddress.City;
            a.Country = codm.shippingAddress.Country;
            a.PhoneNumber = codm.shippingAddress.PhoneNumber;
            a.FirstName = codm.shippingAddress.FirstName;
            a.LastName = codm.shippingAddress.LastName;
            a.Zipcode = codm.shippingAddress.Zipcode;
            a.Province = codm.shippingAddress.Province;

            a.customerAddressId = Guid.NewGuid();
            a.CreatedOn = DateTime.UtcNow;
            a.UpdatedOn = DateTime.UtcNow;
            a.userId = codm.userId;

            using (var conn = Database.Connection)
            {
                var newAddress = await conn.QueryAsync<CustomerAddress>("CustomerAddressInsert", new
                {
                    a.customerAddressId,
                    a.userId,
                    a.FirstName,
                    a.LastName,
                    a.PhoneNumber,
                    a.Address1,
                    a.Address2,
                    a.City,
                    a.Province,
                    a.Country,
                    a.Zipcode,
                    a.UpdatedOn,
                    a.CreatedOn

                }, commandType: CommandType.StoredProcedure);

                if (newAddress.Count() > 0)
                {
                    return newAddress.AsList()[0];
                }
                return null;
            }
        }

        //public static async Task<Address> UpdateAddress(Address a)
        //{
        //    using (var conn = Database.Connection)
        //    {
        //        var newAddress = await conn.QueryAsync<Address>("AddressUpdate", new
        //        {
        //            a.AddressId,
        //            a.FirstName,
        //            a.LastName,
        //            a.Address1,
        //            a.Address2,
        //            a.City,
        //            a.State,
        //            a.Country,
        //            a.PhoneNumber

        //        }, commandType: CommandType.StoredProcedure);

        //        if (newAddress.Count() > 0)
        //        {
        //            return newAddress.AsList()[0];
        //        }
        //        return null;
        //    }
        //}

        //public static async Task<Address> DeleteAddress(Address a)
        //{
        //    using (var conn = Database.Connection)
        //    {
        //        var newAddress = await conn.QueryAsync<Address>("AddressDelete", new
        //        {
        //            a.AddressId

        //        }, commandType: CommandType.StoredProcedure);

        //        if (newAddress.Count() > 0)
        //        {
        //            return newAddress.AsList()[0];
        //        }
        //        return null;
        //    }
        //}
    }
}
