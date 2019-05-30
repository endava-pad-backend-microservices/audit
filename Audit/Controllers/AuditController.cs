﻿using System;
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
        static public async Task<JsonResult> Get()
        {
            return new JsonResult(await new Repository.AuditRepository().FindAll().ConfigureAwait(true));
        }
        // GET: Audit/id
        [HttpGet("{id}")]
        static public async Task<JsonResult> GetbyId(string id)
        {
            return new JsonResult(await new Repository.AuditRepository().FindById(id).ConfigureAwait(true));
        }

        // POST: Audit
        [HttpPost]
        static public async Task<JsonResult> Post([FromBody] Entity.Audit value)
        {
            return new JsonResult(await new Repository.AuditRepository().Create(value).ConfigureAwait(true));
        }

        // POST: Audit/id
        [HttpPut]
        static public async Task<JsonResult> Put([FromBody] Entity.Audit value)
        {
            return new JsonResult(await new Repository.AuditRepository().Update(value).ConfigureAwait(true));
        }

        // DELETE: Audit/5
        [HttpDelete("{id}")]
        static public async Task<bool> Delete(string id)
        {
            var repo = new Repository.AuditRepository();
            var doc = await repo.FindById(id,false).ConfigureAwait(true);
            repo.Delete(doc);
            if (doc != null)
                return true;
            return false;
        }
    }
}