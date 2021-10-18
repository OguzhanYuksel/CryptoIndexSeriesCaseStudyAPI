using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.Trade
{
    public class CoinBaseProTradeItem
    {
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("trade_id")]
        public long TradeId { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }// o anki bir bitcoin fiyatı
        [JsonProperty("size")]
        public string Size { get; set; }// kaç bitcoin alındığı
        [JsonProperty("side")]
        public string Side { get; set; }
    }
}
