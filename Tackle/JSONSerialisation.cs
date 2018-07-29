using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JSONSerialise
{
    class JSONSerialisation
    {
    }

    class ServerRequest
    {
        public string requestSource;
        public string[] requestParameters;

        public string Serialise(ServerRequest request)
        {
            string json = JsonConvert.SerializeObject(request, Formatting.Indented);
            return json;
        }

    }
}
