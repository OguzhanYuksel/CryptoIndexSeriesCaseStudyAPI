using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.CandleStick;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.OrderBook;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.Trade;
using CryptoIndexSeriesCaseStudyAPI.Clients.Utilities;
using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients
{
    public class CoinBaseProClient
    {
        private readonly HttpClient _client;

        public CoinBaseProClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.exchange.coinbase.com/");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TestCaseStudyAPI", "1.0"));
            _client = client;
        }
        public async Task<List<CoinBaseProTradeItem>> GetTradesBySymbol(string symbol, int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}products/{symbol}/trades?limit={limit}")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<List<CoinBaseProTradeItem>>();

            return response;
        }

        public async Task<List<CoinBaseProCandleStickDataItem>> GetCandleStickDataBySymbol(string symbol, long? timeStamp, string start, string end, int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}products/{symbol}/candles?granularity={Constants.Intervals.CoinBaseProCandleStickDataOneDayInterval}&start={start}&end={end}")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<List<CoinBaseProCandleStickDataItem>>();

            if (limit != null)
                response = response.Take(limit.Value).ToList();

            return response;
        }

        public async Task<CoinBaseProOrderBookResponse> GetOrderBookBySymbol(string symbol, int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}products/{symbol}/book?level={Constants.COINBASEPRO_ORDERBOOK_LEVEL}")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<CoinBaseProOrderBookResponse>();

            if (limit != null)
            {
                response.Asks = response.Asks.Take(limit.Value).ToList();
                response.Bids = response.Bids.Take(limit.Value).ToList();
            }

            return response;
        }

        public async Task<List<CoinBaseProSymbolItem>> GetSymbols()
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}products")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<List<CoinBaseProSymbolItem>>();
            return response;
        }

        public class CoinBaseProSymbolItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("base_currency")]
            public string BaseCurrency { get; set; }
            [JsonProperty("quote_currency")]
            public string QuoteCurrency { get; set; }
        }
    }
}
