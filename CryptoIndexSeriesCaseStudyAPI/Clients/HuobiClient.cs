using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Huobi;
using CryptoIndexSeriesCaseStudyAPI.Clients.Utilities;

namespace CryptoIndexSeriesCaseStudyAPI.Clients
{
    public class HuobiClient
    {
        private readonly HttpClient _client;

        public HuobiClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.huobi.pro");
            _client = client;
        }
        public async Task<HuobiTradeResponse> GetTradesBySymbol(string symbol, int? limit)
        {
            var requestMessage = new HttpRequestMessage
            (
                HttpMethod.Get,
                new Uri($"{_client.BaseAddress}market/history/trade?symbol={symbol}&size={limit}")//BTCUSDT
            );

            var responseMessage = await _client.SendAsync(requestMessage);
            var response = responseMessage.ContentAsType<HuobiTradeResponse>();

            return response;
        }
    }
}
