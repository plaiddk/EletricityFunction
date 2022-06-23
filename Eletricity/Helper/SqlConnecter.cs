using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Helper
{
    internal class SqlConnecter
    {

        public  static SqlConnection SqlConn()

        { 
           SqlConnection conn = new SqlConnection(@"Data Source=eletricity.database.windows.net;Initial Catalog=EletricityDB;User ID=sqladmin;Password=JollyJumper1;Encrypt=True;TrustServerCertificate=False;Persist Security Info=False");
            return conn;         
          
        }
    }
}
