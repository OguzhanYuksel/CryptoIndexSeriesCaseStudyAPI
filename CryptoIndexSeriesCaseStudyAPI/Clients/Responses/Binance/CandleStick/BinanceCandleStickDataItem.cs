using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.CandleStick
{
    [JsonConverter(typeof(BinanceCandleStickDataJsonConverter))]
    public class BinanceCandleStickDataItem
    {
        public long OpenTime { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
    }

    public class BinanceCandleStickDataJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer,value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jToken = JToken.Load(reader);
            var instance = Activator.CreateInstance(objectType);

            foreach (var itemChild in jToken.Children())
            {
                if (itemChild.Type is JTokenType.Integer)
                {
                    if (itemChild.Path == "[0]")
                    {
                        objectType.GetProperty(nameof(BinanceCandleStickDataItem.OpenTime))?.SetValue(instance, itemChild.Value<long>());
                    }
                }
                else
                {
                    if (itemChild.Path == "[1]")
                    {
                        objectType.GetProperty(nameof(BinanceCandleStickDataItem.Open))?.SetValue(instance, itemChild.Value<string>());
                    }
                    else if (itemChild.Path == "[2]")
                    {
                        objectType.GetProperty(nameof(BinanceCandleStickDataItem.High))?.SetValue(instance, itemChild.Value<string>());
                    }
                    else if (itemChild.Path == "[3]")
                    {
                        objectType.GetProperty(nameof(BinanceCandleStickDataItem.Low))?.SetValue(instance, itemChild.Value<string>());
                    }
                    else if (itemChild.Path == "[4]")
                    {
                        objectType.GetProperty(nameof(BinanceCandleStickDataItem.Close))?.SetValue(instance, itemChild.Value<string>());
                    }
                    else if (itemChild.Path == "[5]")
                    {
                        objectType.GetProperty(nameof(BinanceCandleStickDataItem.Volume))?.SetValue(instance, itemChild.Value<string>());
                    }
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
