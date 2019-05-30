using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Audit.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        // GET: Audit
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(await new Repository.AuditRepository().FindAll().ConfigureAwait(true));
        }
        // GET: Audit/id
        [HttpGet("{id}")]
        public async Task<JsonResult> GetbyId(string id)
        {
            return new JsonResult(await new Repository.AuditRepository().FindById(id,true).ConfigureAwait(true));
        }

        // POST: Audit
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] Entity.Audit value)
        {
            return new JsonResult(await new Repository.AuditRepository().Create(value).ConfigureAwait(true));
        }

        // POST: Audit/id
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] Entity.Audit value)
        {
            return new JsonResult(await new Repository.AuditRepository().Update(value).ConfigureAwait(true));
        }

        // DELETE: Audit/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            var repo = new Repository.AuditRepository();
            var doc = await repo.FindById(id,false).ConfigureAwait(true);
            var result = await repo.Delete(doc);
            return result;
        }
    }
}
