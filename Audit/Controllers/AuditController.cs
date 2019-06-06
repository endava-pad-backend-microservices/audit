using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Audit.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        readonly private Repository.AuditRepository _repository;

        public AuditController(IOptions<EnvironmentConfig> configuration)
        {
            _repository = new Repository.AuditRepository(configuration);
        }

        // GET: Audit
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult(await _repository.FindAll().ConfigureAwait(true));
        }
        // GET: Audit/id
        [HttpGet("{id}")]
        public async Task<JsonResult> GetbyId(string id)
        {
            return new JsonResult(await _repository.FindById(id,true).ConfigureAwait(true));
        }

        // POST: Audit
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] Entity.Audit value)
        {
            return new JsonResult(await _repository.Create(value).ConfigureAwait(true));
        }

        // PUT: Audit/id
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] Entity.Audit value)
        {
            return new JsonResult(await _repository.Update(value).ConfigureAwait(true));
        }

        // DELETE: Audit/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            var doc = await _repository.FindById(id,false).ConfigureAwait(true);
            var result = await _repository.Delete(doc).ConfigureAwait(true);
            return result;
        }
    }
}
