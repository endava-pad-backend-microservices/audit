using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Services
{
    interface IService
    {
        Task<Dictionary<string, string>> Get();

    }
}
