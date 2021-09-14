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
                        c.Birthdate,
                        c.Gender,
                        c.TimeZoneId,
                        c.UserId,
                        c.IsMember
                        
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


        public static async Task<Order> InsertCustomerOrder(CustomerOrderDataModel cdm, Guid customerId)
        {
            if (cdm != null)
            {

                CustomerOrderDataModel codm = new CustomerOrderDataModel();
                List<OrderItem> items = new List<OrderItem>();
                Order order = new Order();
                order.OrderId = Guid.NewGuid();
                order.OrderNumber = "TestOrder-" + order.OrderId;
                order.CustomerId = customerId;
                DateTime temp = DateTime.UtcNow;
                order.CreatedOn = temp;
                order.UpdatedOn = temp;
                items = cdm.orderItems;
                
                //insert customer
                using (var conn = Business.Database.Connection)
                {
                    foreach (var item in items)
                    {
                        item.CustomerId = customerId;
                        item.OrderItemId = Guid.NewGuid();
                        item.OrderId = order.OrderId;
                        //TODO: assign productID
                        

                        var newOrderItem = await conn.QueryAsync<OrderItem>("OrderItemInsert", new
                        {
                            item.OrderItemId,
                            item.CustomerId,
                            item.StripePriceID,
                            item.OrderId,
                            item.ProductId,
                            item.PrepaidAlterations,
                            item.PurchasedInsurance,
                            item.QuantityOrdered,
                            item.UnitPrice,
                            item.UnitPriceInclTax,
                            item.TaxRate,
                            item.ProductTitle,
                            item.ProductDescription
                        });
                    }
                    order.OrderItems = items;
                    order.OrderStatus = "OrderReceived";

                    var newOrder = await conn.QueryAsync<CustomerOrder>("OrderInsert", new
                    {
                        order.OrderId,
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

        //public static async Task<CustomerOrder> InsertCustomerOrder(CustomerOrderDataModel cdm, Guid customerId)
        //{
        //    if (cdm != null)
        //    {

        //        CustomerOrderDataModel codm = new CustomerOrderDataModel();
        //        List<OrderItem> items = new List<OrderItem>();
        //        CustomerOrder order = new CustomerOrder();
        //        order.OrderId = Guid.NewGuid();
        //        order.OrderNumber = "TestOrder-" + order.OrderId;
        //        order.CustomerId = customerId;
        //        DateTime temp = DateTime.UtcNow;
        //        order.CreatedOn = temp;
        //        order.UpdatedOn = temp;
        //        items = cdm.orderItems;
        //        List<OrderItem> returnedItems = new List<OrderItem>();

        //        //insert customer
        //        using (var conn = Business.Database.Connection)
        //        {
        //            foreach (var item in items)
        //            {
        //                item.CustomerId = customerId;
        //                item.OrderItemId = Guid.NewGuid();
        //                item.OrderId = order.OrderId;

        //                var newOrderItem = await conn.QueryAsync<OrderItem>("OrderItemInsert", new
        //                {
        //                    item.OrderItemId,
        //                    item.CustomerId,
        //                    item.StripePriceID,
        //                    item.OrderId
        //                },
        //                commandType: CommandType.StoredProcedure);
        //                returnedItems.Add(item);
        //            }
        //            order.orderItems = items;

        //            var newOrder = await conn.QueryAsync<CustomerOrder>("OrderInsert", new
        //            {
        //                order.OrderId,
        //                order.OrderNumber,
        //                order.CustomerId,
        //                order.CreatedOn,
        //                cdm.userId
        //            },
        //                commandType: CommandType.StoredProcedure);

        //            if (returnedItems.Count() > 0)
        //            {
        //                return order;
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