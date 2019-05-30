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
        Connection conn = new Connection();
        public async Task<List<Entity.Audit>> FindAll()
        {
            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().ToListAsync().ConfigureAwait(true);
            conn.Dispose();
            return result;
        }
        public async Task<Entity.Audit> FindById(string id,bool closeConnection = true)
        {
            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().FindAsync(id).ConfigureAwait(true);
            if(closeConnection)
            conn.Dispose();
            return result;
        }
        public async Task<bool> Create(Entity.Audit doc)
        {

            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().CreateAsync(doc).ConfigureAwait(true);
            conn.Dispose();
            if (result != null)
                return true;
            return false;
        }
        public async Task<bool> Update(Entity.Audit doc)
        {
            var result = await conn.GetConnection().GetDatabase<Entity.Audit>().CreateOrUpdateAsync(doc).ConfigureAwait(true);
            conn.Dispose();
            if (result != null)
                return true;
            return false;
        }

        public async void Delete(Entity.Audit doc)
        {
            await conn.GetConnection().GetDatabase<Entity.Audit>().DeleteAsync(doc).ConfigureAwait(true);
            conn.Dispose();
        }
    }
}
