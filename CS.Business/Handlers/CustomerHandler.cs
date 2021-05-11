using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CS.Business.Handlers
{
    using CS.Common.Models;
    using CS.Common.Utility;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;

    public class CustomerHandler
    {

        public static async Task<Customer> InsertCustomer(CustomerOrderDataModel cdm)
        {
            if (cdm != null)
            {
                Customer c = new Customer();
                c.CustomerId = Guid.NewGuid();
                c.Email = cdm.measurements.email;
                c.MeasurementsId = Guid.NewGuid();

                //insert customer
                using (var conn = Business.Database.Connection)
                {
                    var newCustomer = await conn.QueryAsync<Customer>("CustomerOrderInsert", new
                    {
                        c.CustomerId,
                        c.Email,
                        c.MeasurementsId
                    },
                     commandType: CommandType.StoredProcedure);
                    if (newCustomer.Count() > 0)
                    {
                        return newCustomer.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }


        public static async Task<List<OrderItem>> InsertCustomerOrder(CustomerOrderDataModel cdm, Guid customerId)
        {
            if (cdm != null)
            {

                CustomerOrderDataModel codm = new CustomerOrderDataModel();
                List<OrderItem> items = new List<OrderItem>();
                items = cdm.orderItems;
                Customer c = new Customer();
                List<OrderItem> returnedItems = new List<OrderItem>();
                
                //insert customer
                using (var conn = Business.Database.Connection)
                {
                    foreach(var item in items)
                    {
                        item.CustomerId = customerId;
                        item.OrderItemId = Guid.NewGuid();

                        var newOrder = await conn.QueryAsync<OrderItem>("NewOrderInsert", new
                        {
                            item.OrderItemId,
                            item.CustomerId,
                            item.count,
                            item.description,
                            item.StripePriceID,
                            item.StripeID,
                            item.Title
                        },
                        commandType: CommandType.StoredProcedure);
                        returnedItems.Add(item);
                    }

                    if (returnedItems.Count() > 0)
                    {
                        return returnedItems;
                    }
                    return null;
                }
            }
            return null;
        }

        //public static async Task<Customer> InsertCustomer(CustomerDataModel cdm)
        //{
        //    if(cdm.Customer != null)
        //    {

        //    Customer c = cdm.Customer;
        //    c.CustomerId = Guid.NewGuid();
        //    c.Active = true;

        //        using (var conn = Business.Database.Connection)
        //        {
        //            var newCustomer = await conn.QueryAsync<Customer>("CustomerInsert", new
        //            {
        //                c.CustomerId,
        //                c.Email,
        //                c.Active,
        //                c.Username,
        //                c.FirstName,
        //                c.LastName,
        //                c.Birthdate,
        //                c.Gender,
        //                c.TimeZoneId,
        //                c.InstagramHandle,
        //                c.StripeUserID
        //            },
        //             commandType: CommandType.StoredProcedure);
        //            if (newCustomer.Count() > 0)
        //            {
        //                return newCustomer.AsList()[0];
        //            }
        //            return null;
        //        }
        //    }
        //    return null;
        //}

        public static async Task<Customer> UpdateCustomer(CustomerDataModel cdm)
        {
            if (cdm.Customer != null)
            {

                Customer c = cdm.Customer;

                using (var conn = Business.Database.Connection)
                {
                    var newCustomer = await conn.QueryAsync<Customer>("CustomerUpdate", new
                    {
                        c.CustomerId,
                        c.MeasurementsId,
                        c.Email,
                        c.Active,
                        c.Username,
                        c.FirstName,
                        c.LastName,
                        c.Birthdate,
                        c.Gender,
                        c.TimeZoneId,
                        c.InstagramHandle,
                        c.StripeUserID
                    },
                     commandType: CommandType.StoredProcedure);
                    if (newCustomer.Count() > 0)
                    {
                        return newCustomer.AsList()[0];
                    }
                    return null;
                }
            }
            return null;
        }

        public static async Task<bool> DeleteCustomer(Customer c)
        {
            if (c != null)
            {
                using (var conn = Business.Database.Connection)
                {
                    var newCustomer = await conn.ExecuteAsync("CustomerDelete", new
                    {
                        c.CustomerId,
                        c.MeasurementsId
                    },
                     commandType: CommandType.StoredProcedure);
                    //TODO: Verify delete was successful
                    return true;
                }
            }
            return false;
        }

        public static async Task<CustomerDataModel> GetCustomer(string email)
        {
            //TODO: replace ids with real data ids and attempt to return multiple tables into cdm object
            Customer c = new Customer();
            c.CustomerId = Guid.NewGuid();
            Measurements m = new Measurements();
            m.MeasurementsId = Guid.NewGuid();
            using (var conn = Business.Database.Connection)
            {
                var newCustomer = await conn.QueryAsync<CustomerDataModel>("Test", new
                {
                    c.Email
                },
                    commandType: CommandType.StoredProcedure);
                if (newCustomer.Count() > 0)
                {
                    return newCustomer.AsList()[0];
                }
                return null;
            }
            return null;
        }
    }
}