using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Configuration
{
    public class SQLSettings
    {
        public string SQLUser { get; set; }

        public string SQLPassword { get; set; }


        public string SQLServer { get; set; }
        public string SQLDatabase { get; set; }

    }

    public class BlobStorageSettings
    {
        public string StorageKey { get; set; }
        public string StorageName { get; set; }
    }

    public class ELOverblikSettings
    {
        public string MeteringKey { get; set; }
        public string MeteringToken { get; set; }
    }
}
