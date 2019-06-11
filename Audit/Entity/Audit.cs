
using Audit.Utils;
using CouchDB.Driver.Types;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Entity
{
    public class Audit : CouchDocument
    {
        public string Date { get; set; }
        public string Verb { get; set; }
        public string Endpoint { get; set; }
        public string Body { get; set; }
        public string Response { get; set; }
        static public string FilterBody(string jString)
        {
            var jObject = new JsonObject();
            if (jString.Length == 0 || jString == null)
            {
                return jString;
            }
            try
            {
                jObject = SimpleJson.DeserializeObject<JsonObject>(jString.ToLowerInvariant());
            }
            catch
            {
                return jString;
            }
            if (Startup.StaticConfig[Constants.auditNotSave] != null)
            {
                var notToSave = Startup.StaticConfig[Constants.auditNotSave].Split(",");
                foreach (var token in notToSave)
                {
                    if (jObject.ContainsKey(token))
                    {
                        jObject[token] = "";
                    }
                }
            }
            return SimpleJson.SerializeObject(jObject);
        }
    }
}
