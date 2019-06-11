using Audit.Utils;
using CouchDB.Driver;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Repository
{
    public class Repository
    {
        public CouchClient ConnectionDB { get; set; }
        readonly private EnvironmentConfig _configuration;
        readonly private ILogger _logger;
        public Repository(EnvironmentConfig configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            if (_configuration.COUCHDB_URL != null)
            {
                string url = "http://" + _configuration.COUCHDB_URL + ":" + _configuration.COUCHDB_PORT;
                this.ConnectionDB = new CouchClient(url,
                      s => s.UseBasicAuthentication(_configuration.COUCHDB_USER, _configuration.COUCHDB_PASSWORD));
            }
            else
            {
                this.ConnectionDB = new CouchClient("http://" + Startup.StaticConfig["DBconnection:url"] + ":" + Startup.StaticConfig["DBconnection:port"],
                  s => s.UseBasicAuthentication(Startup.StaticConfig["DBconnection:user"], Startup.StaticConfig["DBconnection:password"]));
            }
            _logger.LogInformation(String.Format("DB at: {0}.", this.ConnectionDB.ConnectionString));
        }

        public async Task<Boolean> InitializateDb()
        {
            var isUp = false;
            while (!isUp)
            {
                _logger.LogInformation("Checking for DB");
                System.Threading.Thread.Sleep(10000);
                isUp = IsUp().Result;
            }
            await CreateDd().ConfigureAwait(true);
            var rCouch = new RestSharp.RestClient(ConnectionDB.ConnectionString);
            string user = "";
            string password = "";
            if (_configuration.COUCHDB_URL != null)
            {
                user = _configuration.COUCHDB_USER;
                password = _configuration.COUCHDB_PASSWORD;
            }
            else
            {
                user = Startup.StaticConfig["DBconnection:user"];
                password = Startup.StaticConfig["DBconnection:password"];
            }
            var auth = new RestSharp.Authenticators.HttpBasicAuthenticator(user, password);
            rCouch.Authenticator = auth;
            try
            {
                rCouch.ExecuteAsync(new RestSharp.RestRequest("/_users"), (resp, opt) => { }, RestSharp.Method.PUT);
            }
            catch
            {
                _logger.LogInformation("Users table creation skip.");
            }
            ConnectionDB.Dispose();
            return true;
        }

        public async Task<bool> CreateDd()
        {
            try
            {
                _logger.LogInformation("Creating tables.");
                await ConnectionDB.CreateDatabaseAsync<Entity.Audit>().ConfigureAwait(true);
                return true;
            }
            catch
            {
                _logger.LogInformation("Tables creation skip.");
                return false;
            }
        }
        public async Task<bool> IsUp()
        {
            try
            {
                if (await ConnectionDB.IsUpAsync())
                {
                    return true;
                }
                return false;
            }
            catch
            {
                _logger.LogWarning("Fail to connect to DB.");
                return false;
            }
        }
    }
}
