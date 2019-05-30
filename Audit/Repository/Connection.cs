using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouchDB.Driver;

namespace Audit.Repository
{
    public class Connection 
    {
        private CouchClient ConnectionDB { get; set; }
        public Connection()
        {
            try
            {
                this.ConnectionDB = new CouchClient("http://"+ Startup.StaticConfig["DBconnection:url"]+":"+ Startup.StaticConfig["DBconnection:port"],
                    s => s.UseBasicAuthentication(Startup.StaticConfig["DBconnection:user"], Startup.StaticConfig["DBconnection:password"]));
            }
            catch (CouchDB.Driver.Exceptions.CouchException)
            {
                this.ConnectionDB.Dispose();
            }
        }

        public CouchClient GetConnection()
        {
            return this.ConnectionDB;
        }

        public bool IsUp()
        {
            try
            {
                if (this.ConnectionDB.IsUpAsync().Result)
                {
                    return true;
                }
                return false;
            }catch(CouchDB.Driver.Exceptions.CouchException e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
        }

        public void DisposeConnection()
        {
            this.ConnectionDB.Dispose();
        }


    }

}
