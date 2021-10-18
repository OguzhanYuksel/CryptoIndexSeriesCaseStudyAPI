using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.OrderBook
{
    [JsonConverter(typeof(BinanceOrderBookDataJsonConverter))]
    public class BinanceOrderBookResponse
    {
        public List<BinanceOrderBookDataItem> Bids { get; set; }
        public List<BinanceOrderBookDataItem> Asks { get; set; }
    }

    public class BinanceOrderBookDataJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jToken = JToken.Load(reader);
            var instance = Activator.CreateInstance<BinanceOrderBookResponse>();
            instance.Bids = new List<BinanceOrderBookDataItem>();
            instance.Asks = new List<BinanceOrderBookDataItem>();

            var bidsToken = jToken.SelectToken("bids");
            var asksToken = jToken.SelectToken("asks");

            if (bidsToken != null)
            {
                foreach (var child in bidsToken.Children())
                {
                    var orderBookDataItem = Activator.CreateInstance<BinanceOrderBookDataItem>();
                    orderBookDataItem.Price = child.First?.Value<string>();
                    orderBookDataItem.Quantity = child.Last?.Value<string>();
                    instance.Bids.Add(orderBookDataItem);
                }
            }

            if (asksToken != null)
            {
                foreach (var child in asksToken.Children())
                {
                    var orderBookDataItem = Activator.CreateInstance<BinanceOrderBookDataItem>();
                    orderBookDataItem.Price = child.First?.Value<string>();
                    orderBookDataItem.Quantity = child.Last?.Value<string>();
                    instance.Asks.Add(orderBookDataItem);
                }
            }
            return instance;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}
