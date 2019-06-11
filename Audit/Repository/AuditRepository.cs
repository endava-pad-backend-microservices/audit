using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Utils;
using CouchDB.Driver;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Audit.Repository
{
    class AuditRepository : IRepositoryBase<Entity.Audit>
    {
        readonly private ILogger _logger;
        readonly private Repository _repo;
        public AuditRepository(IOptions<EnvironmentConfig> configuration,ILogger logger)
        {
            _logger = logger;
            _repo = new Repository(configuration.Value, _logger);
        }
        public async Task<List<Entity.Audit>> FindAll()
        {
            var result = await _repo.ConnectionDB.GetDatabase<Entity.Audit>().ToListAsync().ConfigureAwait(true);
            _repo.ConnectionDB.Dispose();
            return result;
        }
        public async Task<Entity.Audit> FindById(string id, bool closeConnection)
        {
            var result = await _repo.ConnectionDB.GetDatabase<Entity.Audit>().FindAsync(id).ConfigureAwait(true);
            if (closeConnection)
            {
                _repo.ConnectionDB.Dispose();
            }
            return result;
        }
        public async Task<Entity.Audit> Create(Entity.Audit doc)
        {
            var result = await _repo.ConnectionDB.GetDatabase<Entity.Audit>().CreateAsync(Filter(doc)).ConfigureAwait(true);
            _repo.ConnectionDB.Dispose();
            return result;
        }
        public async Task<Entity.Audit> Update(Entity.Audit doc)
        {
            var result = await _repo.ConnectionDB.GetDatabase<Entity.Audit>().CreateOrUpdateAsync(Filter(doc)).ConfigureAwait(true);
            _repo.ConnectionDB.Dispose();
            return result;
        }

        public async Task<bool> Delete(Entity.Audit doc)
        {
            await _repo.ConnectionDB.GetDatabase<Entity.Audit>().DeleteAsync(doc).ConfigureAwait(true);
            _repo.ConnectionDB.Dispose();
            return true;
        }

        static private Entity.Audit Filter(Entity.Audit doc)
        {
            doc.Body = Entity.Audit.FilterBody(doc.Body);
            doc.Response = Entity.Audit.FilterBody(doc.Response);
            return doc;
        }

    }
}
