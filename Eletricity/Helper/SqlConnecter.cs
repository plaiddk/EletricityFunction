using Eletricity.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Helper
{
    public class SqlConnecter
    {
        private readonly SQLSettings _connectionStrings;

        public SqlConnecter(IOptions<SQLSettings> connectionStringSettings)
        {

            _connectionStrings = connectionStringSettings?.Value ?? throw new ArgumentNullException(nameof(connectionStringSettings));

        }
        public  SqlConnection SqlConn()

        { 
           SqlConnection conn = new SqlConnection($@"Data Source={_connectionStrings.SQLServer};Initial Catalog={_connectionStrings.SQLDatabase};User ID={_connectionStrings.SQLUser};Password={_connectionStrings.SQLPassword};Encrypt=True;TrustServerCertificate=False;Persist Security Info=False");
            return conn;         
          
        }
    }
}
