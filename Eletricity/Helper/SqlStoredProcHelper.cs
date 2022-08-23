using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Helper
{
    public class SqlExecuteHelper
    {
        private readonly SqlConnecter _sqlConn;
        private readonly ILogger<SqlExecuteHelper> _logger;

            public SqlExecuteHelper(SqlConnecter sqlConn, ILogger<SqlExecuteHelper> logger)
        {
            _sqlConn = sqlConn;
            _logger = logger;
        }
        public string Execute(string query)
        {
            try
            {
                var conn = _sqlConn.SqlConn();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

               _logger.LogInformation(ex.Message);
            }


            return "succes";


        }
    }
}
