using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Audit.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {

        readonly private string environment = Startup.StaticConfig[Constants.environment];
        // PUT: Refresh/
        [HttpPut]
        public async Task<JsonResult> Put()
        {
            Startup.StaticConfig = Startup.AppConfiguration(Startup.StaticConfig);
            return new JsonResult(environment);
        }
    }
}
