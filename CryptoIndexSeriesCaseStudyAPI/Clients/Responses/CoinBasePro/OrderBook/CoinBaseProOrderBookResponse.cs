using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.OrderBook
{
    [JsonConverter(typeof(CoinBaseProOrderBookDataJsonConverter))]
    public class CoinBaseProOrderBookResponse
    {
        public List<CoinBaseProOrderBookDataItem> Bids { get; set; }
        public List<CoinBaseProOrderBookDataItem> Asks { get; set; }
    }
    public class CoinBaseProOrderBookDataJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jToken = JToken.Load(reader);
            var instance = Activator.CreateInstance<CoinBaseProOrderBookResponse>();
            instance.Bids = new List<CoinBaseProOrderBookDataItem>();
            instance.Asks = new List<CoinBaseProOrderBookDataItem>();

            var bidsToken = jToken.SelectToken("bids");
            var asksToken = jToken.SelectToken("asks");

            if (bidsToken != null)
            {
                foreach (var child in bidsToken.Children())
                {
                    var orderBookDataItem = Activator.CreateInstance<CoinBaseProOrderBookDataItem>();
                    orderBookDataItem.Price = child.First?.Value<string>();
                    orderBookDataItem.Quantity = child.First?.Next?.Value<string>();
                    instance.Bids.Add(orderBookDataItem);
                }
            }

            if (asksToken != null)
            {
                foreach (var child in asksToken.Children())
                {
                    var orderBookDataItem = Activator.CreateInstance<CoinBaseProOrderBookDataItem>();
                    orderBookDataItem.Price = child.First?.Value<string>();
                    orderBookDataItem.Quantity = child.First?.Next?.Value<string>();
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
