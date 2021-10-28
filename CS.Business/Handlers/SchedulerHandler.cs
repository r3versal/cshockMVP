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

    public class SchedulerHandler
    {
        public static async Task<Fitting> InsertFitting(Fitting fitting)
        {
            using (var conn = Business.Database.Connection)
            {
                fitting.createdOn = DateTime.UtcNow;
                fitting.fittingId = Guid.NewGuid();
                var returnedFitting = await conn.QueryAsync<Fitting>("FittingInsert", new
                {
                    fitting.fittingId,
                    fitting.userId,
                    fitting.orderId,
                    fitting.isVirtual,
                    fitting.timeRequested,
                    fitting.dateRequested,
                    fitting.timeZone,
                    fitting.createdOn

                }, commandType: CommandType.StoredProcedure);
                if (returnedFitting.AsList().Count() > 0)
                {
                    return returnedFitting.AsList()[0];
                }
                return null;
            }
        }
    }
}
