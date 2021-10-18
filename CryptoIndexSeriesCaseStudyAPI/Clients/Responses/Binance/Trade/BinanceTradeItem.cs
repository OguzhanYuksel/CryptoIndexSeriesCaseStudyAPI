using System;
using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.Trade
{
    public class BinanceTradeItem
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; } //base currency price bir bitcoin o anki fiyatı
        [JsonProperty("qty")]
        public string Qty { get; set; } //base currency quantity tradede kaç bitcoin alındığı
        [JsonProperty("quoteQty")]
        public string QuoteQty { get; set; }//kaç dolar tuttuğu
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("isBuyerMaker")]
        public bool IsBuyerMaker { get; set; }
    }
}
