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

    public class OrderHandler
    {
        public static async Task<List<CustomerOrder>> OrderHistory(Guid userId)
        {
            List<CustomerOrder> orders = new List<CustomerOrder>();
            using (var conn = Business.Database.Connection)
            {
                var foundOrders = await conn.QueryAsync<CustomerOrder>("SELECT * FROM customerOrder WHERE userId = " + "'" + userId + "'");
                if (foundOrders.Count() > 0)
                {
                    return foundOrders.AsList();
                }
                return orders;
            }
        }
    }
}
