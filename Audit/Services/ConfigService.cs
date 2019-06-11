using Audit.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Services
{
    public class ConfigService : IService
    {
        readonly private RestClient client = new RestClient("http://" + Startup.StaticConfig[Constants.configUrl]);
        public async Task<Dictionary<string, string>> Get()
        {
            string endpoint = Constants.configEndpoint + Startup.StaticConfig[Constants.environment] + "-" + Startup.StaticConfig[Constants.environment] + ".json";
            var request = new RestRequest(endpoint, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            return ParseResponse(await client.ExecuteTaskAsync(request).ConfigureAwait(true));
        }

        static private Dictionary<string, string> ParseResponse(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resp = SimpleJson.DeserializeObject<JsonObject>(response.Content);
                return Utils.Utils.toDic(resp.ToString());
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
