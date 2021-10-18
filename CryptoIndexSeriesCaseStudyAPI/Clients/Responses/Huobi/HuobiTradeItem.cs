using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Huobi
{
    public class HuobiTradeItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("ts")]
        public long TimeStamp { get; set; }
        [JsonProperty("trade-id")]
        public long TradeId { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("price")] 
        public decimal Price { get; set; }
        [JsonProperty("direction")]
        public string Direction { get; set; }
    }
}
