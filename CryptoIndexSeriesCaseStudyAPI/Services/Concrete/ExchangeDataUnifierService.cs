using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Binance.Net;
using Binance.Net.Interfaces;
using Coinbase.Pro.Models;
using Coinbase.Pro.WebSockets;
using CryptoIndexSeriesCaseStudyAPI.Clients;
using CryptoIndexSeriesCaseStudyAPI.Controllers;
using CryptoIndexSeriesCaseStudyAPI.DTOs;
using CryptoIndexSeriesCaseStudyAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using BinanceClient = CryptoIndexSeriesCaseStudyAPI.Clients.BinanceClient;

namespace CryptoIndexSeriesCaseStudyAPI.Services.Concrete
{
    public class ExchangeDataUnifierService : IExchangeDataUnifierService
    {
        private readonly ILogger<UnifiedExchangeDataController> _logger;
        private readonly BinanceClient _binanceClient;
        private readonly CoinBaseProClient _coinBaseProClient;
        private readonly HuobiClient _huobiClient;
        private readonly IMapper _mapper;

        public ExchangeDataUnifierService(ILogger<UnifiedExchangeDataController> logger, BinanceClient binanceClient, CoinBaseProClient coinBaseProClient, HuobiClient huobiClient, IMapper mapper)
        {
            _logger = logger;
            _binanceClient = binanceClient;
            _coinBaseProClient = coinBaseProClient;
            _huobiClient = huobiClient;
            _mapper = mapper;
        }
        public async Task<List<UnifiedTradeDataItem>> GetUnifiedTradesBySymbol(string symbol, int? limit)
        {
            var result = new List<UnifiedTradeDataItem>();
            if (SymbolDictionaries.BinanceSymbolDictionary.ContainsKey(symbol))
            {
                var responseBinance = await _binanceClient.GetTradesBySymbol(SymbolDictionaries.BinanceSymbolDictionary[symbol], limit ?? 10);
                result = _mapper.Map<List<UnifiedTradeDataItem>>(responseBinance, opts =>
                {
                    opts.ConstructServicesUsing(x => new UnifiedTradeDataItem(Constants.Exchanges.BINANCE, symbol));
                });
            }

            if (SymbolDictionaries.CoinBaseProSymbolDictionary.ContainsKey(symbol))
            {
                var responseCoinBasePro = await _coinBaseProClient.GetTradesBySymbol(SymbolDictionaries.CoinBaseProSymbolDictionary[symbol], limit ?? 10);
                var resultListCoinBasePro = _mapper.Map<List<UnifiedTradeDataItem>>(responseCoinBasePro, opts =>
                {
                    opts.ConstructServicesUsing(x => new UnifiedTradeDataItem(Constants.Exchanges.COINBASEPRO, symbol));
                });
                result.AddRange(resultListCoinBasePro);
            }
            return result;
        }

        public async Task<List<UnifiedCandleStickDataItem>> GetCandleStickDataBySymbol(string symbol, long? timeStamp, int? limit, long? startTime, long? endTime)
        {
            var result = new List<UnifiedCandleStickDataItem>();
            if (SymbolDictionaries.BinanceSymbolDictionary.ContainsKey(symbol))
            {
                var responseBinance = await _binanceClient.GetCandleStickDataBySymbol(SymbolDictionaries.BinanceSymbolDictionary[symbol], timeStamp, startTime, endTime, limit);
                result = _mapper.Map<List<UnifiedCandleStickDataItem>>(responseBinance, opts =>
                {
                    opts.ConstructServicesUsing(x => new UnifiedCandleStickDataItem(Constants.Exchanges.BINANCE, symbol));
                });
            }

            if (SymbolDictionaries.CoinBaseProSymbolDictionary.ContainsKey(symbol))
            {
                var responseCoinBasePro = await _coinBaseProClient.GetCandleStickDataBySymbol(SymbolDictionaries.CoinBaseProSymbolDictionary[symbol], timeStamp, startTime.ToString(), endTime.ToString(), limit);

                var resultListCoinBasePro = _mapper.Map<List<UnifiedCandleStickDataItem>>(responseCoinBasePro, opts =>
                {
                    opts.ConstructServicesUsing(x => new UnifiedCandleStickDataItem(Constants.Exchanges.COINBASEPRO, symbol));
                });

                result.AddRange(resultListCoinBasePro);
            }
            return result;
        }

        public async Task<List<UnifiedOrderbookResponse>> GetOrderBookBySymbol(string symbol, int? limit)
        {
            var result = new List<UnifiedOrderbookResponse>();
            if (SymbolDictionaries.BinanceSymbolDictionary.ContainsKey(symbol))
            {
                var responseBinance = await _binanceClient.GetOrderBookBySymbol(SymbolDictionaries.BinanceSymbolDictionary[symbol], limit);
                var resultBinance = _mapper.Map<UnifiedOrderbookResponse>(responseBinance, opts =>
                {
                    opts.ConstructServicesUsing(x => new UnifiedOrderbookResponse(Constants.Exchanges.BINANCE, symbol));
                });
                result.Add(resultBinance);
            }

            if (SymbolDictionaries.CoinBaseProSymbolDictionary.ContainsKey(symbol))
            {
                var responseCoinBasePro = await _coinBaseProClient.GetOrderBookBySymbol(SymbolDictionaries.CoinBaseProSymbolDictionary[symbol], limit);
                var resultCoinBasePro = _mapper.Map<UnifiedOrderbookResponse>(responseCoinBasePro, opts =>
                {
                    opts.ConstructServicesUsing(x => new UnifiedOrderbookResponse(Constants.Exchanges.COINBASEPRO, symbol));
                });
                result.Add(resultCoinBasePro);
            }
            return result;
        }

        public async Task<List<UnifiedOrderbookResponse>> GetLiveOrderBookBySymbol(string symbol)
        {
            List<UnifiedOrderbookResponse> resultList = new List<UnifiedOrderbookResponse>();

            if (SymbolDictionaries.BinanceSymbolDictionary.ContainsKey(symbol))
            {
                var binanceSocketClient = new BinanceSocketClient();
                var callResult = await binanceSocketClient.Spot.SubscribeToOrderBookUpdatesAsync(new List<string>() { SymbolDictionaries.BinanceSymbolDictionary[symbol] }, 100, async (obj) => await BinanceOrderBookUpdateEvent(obj, resultList, symbol));

                if (!callResult.Success)
                {
                    throw new Exception("Binance WebSocket connection error.");
                }
                await binanceSocketClient.UnsubscribeAsync(callResult.Data.Id);
            }

            if (SymbolDictionaries.CoinBaseProSymbolDictionary.ContainsKey(symbol))
            {
                CoinbaseProWebSocket coinbaseProWebSocket = new CoinbaseProWebSocket(new WebSocketConfig()
                {
                    UseTimeApi = true,
                    SocketUri = Constants.COINBASEPRO_SOCKET_URI
                });

                var result = await coinbaseProWebSocket.ConnectAsync();
                if (!result.Success)
                {
                    throw new Exception("CoinBasePro WebSocket connection error.");
                }

                var sub = new Subscription()
                {
                    ProductIds = { SymbolDictionaries.CoinBaseProSymbolDictionary[symbol] },
                    Channels =
                    {
                        "level2",
                        "heartbeat",
                        JObject.FromObject(new Channel()
                        {
                            Name = "ticker",
                            ProductIds = { SymbolDictionaries.CoinBaseProSymbolDictionary[symbol] }
                        })
                    }
                };

                await coinbaseProWebSocket.SubscribeAsync(sub);
                
                coinbaseProWebSocket.RawSocket.MessageReceived += async (sender, e) =>
                {
                    await CoinBaseProOrderBookUpdateEvent(sender, e, resultList, symbol);
                };

                sub.ExtraJson.Remove("type");
                coinbaseProWebSocket.Unsubscribe(sub);
            }
            await Task.Delay(TimeSpan.FromSeconds(2));
            return resultList.OrderBy(x => x.Exchange).ToList();
        }

        private async Task CoinBaseProOrderBookUpdateEvent(object sender, MessageReceivedEventArgs e, List<UnifiedOrderbookResponse> unifiedOrderbookResponses, string symbol)
        {
            if (WebSocketHelper.TryParse(e.Message, out var msg))
            {
                if (msg is SnapshotEvent snapshotEvent)
                {
                    var unifiedOrderbookResponseItem = _mapper.Map<UnifiedOrderbookResponse>(snapshotEvent, opts =>
                    {
                        opts.ConstructServicesUsing(x => new UnifiedOrderbookResponse(Constants.Exchanges.COINBASEPRO, symbol));
                    });
                    unifiedOrderbookResponses.Add(unifiedOrderbookResponseItem);
                }
            }
        }

        private async Task BinanceOrderBookUpdateEvent(CryptoExchange.Net.Sockets.DataEvent<IBinanceEventOrderBook> obj, List<UnifiedOrderbookResponse> unifiedOrderbookResponses, string symbol)
        {
            var unifiedOrderbookResponseItem = _mapper.Map<UnifiedOrderbookResponse>(obj.Data, opts =>
            {
                opts.ConstructServicesUsing(x => new UnifiedOrderbookResponse(Constants.Exchanges.BINANCE, symbol));
            });
            unifiedOrderbookResponses.Add(unifiedOrderbookResponseItem);
        }
    }
}
