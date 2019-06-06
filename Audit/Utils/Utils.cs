using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Utils
{
    static public class Utils
    {
        static public Dictionary<string, string> toDic(string jString)
        {
            var jObject = new JsonObject();
            var result = new Dictionary<string, string>();
            try
            {
                jObject = SimpleJson.DeserializeObject<JsonObject>(jString);
                if (jObject == null)
                {
                    var jArray = new JsonArray();
                    jArray = SimpleJson.DeserializeObject<JsonArray>(jString);
                    int cont = 0;
                    foreach (var jSub in jArray)
                    {
                        result.Add(cont.ToString(), jSub.ToString());
                        cont++;
                    }
                    return result;
                }
                else
                {
                    foreach (var jSub in jObject)
                    {
                        var sub_resp = toDic(jObject[jSub.Key].ToString());
                        if (sub_resp.Count == 0)
                        {
                            result.Add(jSub.Key, jSub.Value.ToString());
                        }
                        else
                        {
                            foreach (var jSub_resp in sub_resp)
                            {
                                result.Add(jSub.Key.ToUpperInvariant() + ":" + jSub_resp.Key.ToUpperInvariant(), jSub_resp.Value);
                            }
                        }
                    }
                }
                return result;
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
