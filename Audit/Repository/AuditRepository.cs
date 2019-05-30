using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Entity;
using CouchDB.Driver;

namespace Audit.Repository
{
    class AuditRepository : IRepositoryBase<Entity.Audit>
    {
        readonly Connection conn = new Connection();
        public async Task<List<Entity.Audit>> FindAll()
        {
            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().ToListAsync().ConfigureAwait(true);
            conn.DisposeConnection();
            return result;
        }
        public async Task<Entity.Audit> FindById(string id, bool closeConnection)
        {
            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().FindAsync(id).ConfigureAwait(true);
            if (closeConnection)
            {
                conn.DisposeConnection();
            }
            return result;
        }
        public async Task<bool> Create(Entity.Audit doc)
        {

            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().CreateAsync(doc).ConfigureAwait(true);
            conn.DisposeConnection();
            if (result != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> Update(Entity.Audit doc)
        {
            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().CreateOrUpdateAsync(doc).ConfigureAwait(true);
            conn.DisposeConnection();
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(Entity.Audit doc)
        {
            await conn.GetConnection().GetDatabase<Entity.Audit>().DeleteAsync(doc).ConfigureAwait(true);
            conn.DisposeConnection();
            return true;
        }
    }
}
