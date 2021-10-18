using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.CandleStick;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.OrderBook;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.Trade;
using CryptoIndexSeriesCaseStudyAPI.Clients.Utilities;
using Newtonsoft.Json;

namespace CryptoIndexSeriesCaseStudyAPI.Clients
{
    public class BinanceClient
    {
        private readonly HttpClient _client;
        public BinanceClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://testnet.binancefuture.com");
            _client = client;
        }

        public async Task<List<BinanceTradeItem>> GetTradesBySymbol(string symbol, int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}/fapi/v1/trades?symbol={symbol}&limit={limit}")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<List<BinanceTradeItem>>();

            return response;
        }

        public async Task<List<BinanceCandleStickDataItem>> GetCandleStickDataBySymbol(string symbol, long? timeStamp, long? startTime, long? endTime, int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}/fapi/v1/klines?symbol={symbol}&interval={Constants.Intervals.BinanceCandleStickDataOneDayInterval}&startTime={startTime}&endTime={endTime}&limit={limit}")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<List<BinanceCandleStickDataItem>>();

            return response;
        }

        public async Task<BinanceOrderBookResponse> GetOrderBookBySymbol(string symbol,int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}/fapi/v1/depth?symbol={symbol}&limit={limit}")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<BinanceOrderBookResponse>();

            return response;
        }

        public async Task<BinanceSymbols> GetSymbols()
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}/fapi/v1/exchangeInfo")
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<BinanceSymbols>();
            return response;
        }

        public class BinanceSymbols
        {
            [JsonProperty("symbols")]
            public List<BinanceSymbolItem> Symbols { get; set; }
        }

        public class BinanceSymbolItem
        {
            [JsonProperty("symbol")]
            public string Symbol { get; set; }
            [JsonProperty("baseAsset")]
            public string BaseAsset { get; set; }
            [JsonProperty("quoteAsset")]
            public string QuoteAsset { get; set; }
        }
    }
}
