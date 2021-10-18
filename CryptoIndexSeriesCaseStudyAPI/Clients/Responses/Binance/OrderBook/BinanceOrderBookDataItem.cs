using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.OrderBook
{
    public class BinanceOrderBookDataItem
    {
        public string Price { get; set; }
        public string Quantity { get; set; }
    }
}
