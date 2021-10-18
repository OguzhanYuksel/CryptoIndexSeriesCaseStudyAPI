using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.CandleStick
{
    [JsonConverter(typeof(CoinBaseProCandleStickDataJsonConverter))]
    public class CoinBaseProCandleStickDataItem
    {
        public long OpenTime { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public float Volume { get; set; }
    }
    public class CoinBaseProCandleStickDataJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
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
                        objectType.GetProperty(nameof(CoinBaseProCandleStickDataItem.OpenTime))?.SetValue(instance, itemChild.Value<long>());
                    }
                }
                else
                {
                    if (itemChild.Path == "[1]")
                    {
                        objectType.GetProperty(nameof(CoinBaseProCandleStickDataItem.Low))?.SetValue(instance, itemChild.Value<float>());
                    }
                    else if (itemChild.Path == "[2]")
                    {
                        objectType.GetProperty(nameof(CoinBaseProCandleStickDataItem.High))?.SetValue(instance, itemChild.Value<float>());
                    }
                    else if (itemChild.Path == "[3]")
                    {
                        objectType.GetProperty(nameof(CoinBaseProCandleStickDataItem.Open))?.SetValue(instance, itemChild.Value<float>());
                    }
                    else if (itemChild.Path == "[4]")
                    {
                        objectType.GetProperty(nameof(CoinBaseProCandleStickDataItem.Close))?.SetValue(instance, itemChild.Value<float>());
                    }
                    else if (itemChild.Path == "[5]")
                    {
                        objectType.GetProperty(nameof(CoinBaseProCandleStickDataItem.Volume))?.SetValue(instance, itemChild.Value<float>());
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
