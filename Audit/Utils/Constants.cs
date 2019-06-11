using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Utils
{
    public static class Constants
    {
        public static readonly string auditNotSave = "AUDIT:NOTTOSAVE";
        public static readonly string gatewayUrl = "GATEWAY_URL";
        public static readonly string configUrl = "CONFIG_URL";
        public static readonly string environment = "ENVIRONMENT";
        public static readonly string configEndpoint = "/configuration/";
        public static readonly string eurekaUrl = "EUREKA_URL";
        public static readonly string serverUrl = "SERVER_URL";
        public static readonly string eurekaServiceUrl = "eureka:client:serviceUrl";
        public static readonly string eurekaServiceHostName = "eureka:instance:hostName";
    }
}
