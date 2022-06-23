using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Configuration
{
    public class ConnectionSettings
    {
        public string SQLPassword {get;set;}
    }

    public class ELoverblikAccess
    {
        public string MeteringKey { get; set; }
        public string MeteringToken { get; set; }
    }
}
