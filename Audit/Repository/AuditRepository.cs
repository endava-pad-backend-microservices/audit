using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Entity;
using Audit.Utils;
using CouchDB.Driver;
using Microsoft.Extensions.Options;

namespace Audit.Repository
{
    class AuditRepository : IRepositoryBase<Entity.Audit>
    {
        private CouchClient ConnectionDB { get; set; }

        readonly private EnvironmentConfig _configuration;

        public AuditRepository(IOptions<EnvironmentConfig> configuration)
        {
            _configuration = configuration.Value;
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
        }
        public async Task<List<Entity.Audit>> FindAll()
        {
            var result = await ConnectionDB.GetDatabase<Entity.Audit>().ToListAsync().ConfigureAwait(true);
            ConnectionDB.Dispose();
            return result;
        }
        public async Task<Entity.Audit> FindById(string id, bool closeConnection)
        {
            var result = await ConnectionDB.GetDatabase<Entity.Audit>().FindAsync(id).ConfigureAwait(true);
            if (closeConnection)
            {
                ConnectionDB.Dispose();
            }
            return result;
        }
        public async Task<Entity.Audit> Create(Entity.Audit doc)
        {

            var result = await ConnectionDB.GetDatabase<Entity.Audit>().CreateAsync(doc).ConfigureAwait(true);
            ConnectionDB.Dispose();
            return result;
        }
        public async Task<Entity.Audit> Update(Entity.Audit doc)
        {
            var result = await ConnectionDB.GetDatabase<Entity.Audit>().CreateOrUpdateAsync(doc).ConfigureAwait(true);
            ConnectionDB.Dispose();
            return result;
        }

        public async Task<bool> Delete(Entity.Audit doc)
        {
            await ConnectionDB.GetDatabase<Entity.Audit>().DeleteAsync(doc).ConfigureAwait(true);
            ConnectionDB.Dispose();
            return true;
        }

        public async Task<bool> CreateDd()
        {
            await ConnectionDB.CreateDatabaseAsync<Entity.Audit>().ConfigureAwait(true);
            return true;
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
            }catch(CouchDB.Driver.Exceptions.CouchException e)
            {
                return false;
            }
        }
    }
}
