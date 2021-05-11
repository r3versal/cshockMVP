using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Business
{
    using System.Data;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    public class Database
    {
        public static string ConnectionString;
        public static IConfiguration Configuration;
        public static IDbConnection Connection
        {
            get
            {
                if (String.IsNullOrEmpty(ConnectionString))
                {
                    ConnectionString = Configuration.GetConnectionString("Development");
                }

                var conn = new SqlConnection(ConnectionString);
                conn.Open();
                return conn;
            }
        }
    }
}
