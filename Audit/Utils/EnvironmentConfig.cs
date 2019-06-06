using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Utils
{
    public class EnvironmentConfig
    {
        public string COUCHDB_URL { get; set; }
        public string EUREKA_URL {get; set;}
        public string SERVER_URL { get; set; }
        public string COUCHDB_USER { get; set; }
        public string COUCHDB_PASSWORD { get; set; }
        public string COUCHDB_PORT { get; set; }
        public string DOCKERIZE_VERSION { get; set; }
        public string ASPNETCORE_URLS { get; set; }
        public string SWAGGER_PATH { get; set; }
        public string GATEWAY_URL { get; set; }
    }
}
