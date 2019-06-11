using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Audit.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        readonly private Repository.AuditRepository _repository;
        readonly private ILogger _logger;

        public AuditController(IOptions<EnvironmentConfig> configuration, ILogger<AuditController> logger)
        {
            _logger = logger;
            _repository = new Repository.AuditRepository(configuration, _logger);
        }

        // GET: Audit
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            try
            {
                return new JsonResult(await _repository.FindAll().ConfigureAwait(true));
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }
        // GET: Audit/id
        [HttpGet("{id}")]
        public async Task<JsonResult> GetbyId(string id)
        {
            try
            {
                return new JsonResult(await _repository.FindById(id, true).ConfigureAwait(true));
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        // POST: Audit
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] Entity.Audit value)
        {
            try
            {
                return new JsonResult(await _repository.Create(value).ConfigureAwait(true));
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        // PUT: Audit/id
        [HttpPut]
        public async Task<JsonResult> Put([FromBody] Entity.Audit value)
        {
            try
            {
                return new JsonResult(await _repository.Update(value).ConfigureAwait(true));
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        // DELETE: Audit/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            try
            {
                _logger.LogInformation(String.Format("Deleting: {0}", id));
                var doc = await _repository.FindById(id, false).ConfigureAwait(true);
                var result = await _repository.Delete(doc).ConfigureAwait(true);
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
