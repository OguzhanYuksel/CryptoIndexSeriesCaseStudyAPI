using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoIndexSeriesCaseStudyAPI.DTOs;

namespace CryptoIndexSeriesCaseStudyAPI.Services.Interfaces
{
    public interface IExchangeDataUnifierService
    {
        public Task<List<UnifiedTradeDataItem>> GetUnifiedTradesBySymbol(string symbol, int? limit);
        public Task<List<UnifiedCandleStickDataItem>> GetCandleStickDataBySymbol(string symbol, long? timeStamp, int? limit, long? startTime, long? endTime);
        public Task<List<UnifiedOrderbookResponse>> GetOrderBookBySymbol(string symbol, int? limit);
        Task<List<UnifiedOrderbookResponse>> GetLiveOrderBookBySymbol(string symbol);
    }
}
