
using CouchDB.Driver.Types;
using Newtonsoft.Json;
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
    }
}
