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
                c.UserId = cdm.userId;
                c.Email = cdm.measurements.email;
                c.MeasurementsId = Guid.NewGuid();
                c.CreatedOn = DateTime.UtcNow;
                c.UpdatedOn = DateTime.UtcNow;

                //insert customer
                using (var conn = Business.Database.Connection)
                {
                    var newCustomer = await conn.QueryAsync<Customer>("CustomerOrderInsert", new
                    {
                        c.CustomerId,
                        c.Email,
                        c.MeasurementsId,
                        c.FirstName,
                        c.LastName,
                        c.Gender,
                        c.TimeZoneId,
                        c.UserId,
                        c.IsMember,
                        c.CreatedOn,
                        c.UpdatedOn
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


        public static async Task<Order> InsertCustomerOrder(CustomerOrderDataModel cdm)
        {
            if (cdm != null)
            {

                CustomerOrderDataModel codm = new CustomerOrderDataModel();
                List<OrderItem> items = new List<OrderItem>();
                Order order = new Order();
                order.OrderId = Guid.NewGuid();
                order.OrderNumber = "TestOrder-" + order.OrderId;
                if(cdm.userId != null)
                {
                    order.CustomerId = cdm.userId;
                }
                DateTime temp = DateTime.UtcNow;
                order.CreatedOn = temp;
                order.UpdatedOn = temp;
                items = cdm.orderItems;
                
                //insert order
                using (var conn = Business.Database.Connection)
                {
                    foreach (OrderItem item in items)
                    {
                        item.OrderItemId = Guid.NewGuid();
                        item.OrderId = order.OrderId;
                        item.ProductVariantId = item.ProductVariantId;
                        item.CreatedOn = DateTime.UtcNow;
                        item.UpdatedOn = DateTime.UtcNow;

                        var newOrderItem = await conn.QueryAsync<OrderItem>("OrderItemInsert", new
                        {
                            item.OrderItemId,
                            item.StripePriceID,
                            item.OrderId,
                            item.ProductVariantId,
                            item.PrepaidAlterations,
                            item.PurchasedInsurance,
                            item.QuantityOrdered,
                            item.UnitPrice,
                            item.UnitPriceInclTax,
                            item.TaxRate,
                            item.ProductTitle,
                            item.ProductDescription,
                            item.CreatedOn,
                            item.UpdatedOn
                        },
                        commandType: CommandType.StoredProcedure);
                    }
                    order.OrderItems = items;
                    order.OrderStatus = "OrderReceived";

                    var newOrder = await conn.QueryAsync<CustomerOrder>("OrderInsert", new
                    {
                        order.OrderId,
                        order.email,
                        order.OrderNumber,
                        order.CustomerId,
                        order.CreatedOn,
                        order.UpdatedOn,
                        order.TransactionId,
                        order.BillingAddressId,
                        order.ShippingAddressId,
                    },
                        commandType: CommandType.StoredProcedure);

                    if (newOrder.Count() > 0)
                    {
                        return order;
                    }
                    return null;
                }
            }
            return null;
        }

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

        public static async Task<Customer> GetCustomer(string email)
        {
            Customer c = new Customer();
            using (var conn = Business.Database.Connection)
            {
                var customer = await conn.QueryAsync<Customer>("GetCustomer", new
                {
                    email
                },
                    commandType: CommandType.StoredProcedure);
                if (customer.Count() > 0)
                {
                    return customer.AsList()[0];
                }
                return null;
            }
        }
    }
}