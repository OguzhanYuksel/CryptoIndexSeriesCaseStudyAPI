using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Huobi
{
    public class HuobiTradeResponse
    {
        public string ch { get; set; }
        [JsonProperty("data")]
        public List<HuobiTradeItem> Data { get; set; }
    }
}
