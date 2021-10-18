using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CryptoIndexSeriesCaseStudyAPI.DTOs;
using CryptoIndexSeriesCaseStudyAPI.Services.Interfaces;

namespace CryptoIndexSeriesCaseStudyAPI.Controllers
{
    [ApiController]
    [Route("[controller]" + "/" + "[action]")]
    public class UnifiedExchangeDataController : ControllerBase
    {
        private readonly ILogger<UnifiedExchangeDataController> _logger;
        private readonly IExchangeDataUnifierService _exchangeDataUnifierService;

        public UnifiedExchangeDataController(ILogger<UnifiedExchangeDataController> logger, IExchangeDataUnifierService exchangeDataUnifierService)
        {
            _logger = logger;
            _exchangeDataUnifierService = exchangeDataUnifierService;
        }

        [HttpGet]
        public async Task<List<UnifiedTradeDataItem>> GetTradesBySymbol(string symbol, int? limit)
        {
            var result = await _exchangeDataUnifierService.GetUnifiedTradesBySymbol(symbol, limit);
            return result;
        }

        [HttpGet]
        public async Task<List<UnifiedCandleStickDataItem>> GetCandleStickDataBySymbol(string symbol, long? timeStamp, int? limit, long? startTime, long? endTime)
        {
            var result = await _exchangeDataUnifierService.GetCandleStickDataBySymbol(symbol, timeStamp, limit, startTime, endTime);
            return result;
        }

        [HttpGet]
        public async Task<List<UnifiedOrderbookResponse>> GetOrderBookBySymbol(string symbol, int? limit)
        {
            var result = await _exchangeDataUnifierService.GetOrderBookBySymbol(symbol, limit);
            return result;
        }

        [HttpGet]
        public async Task<List<UnifiedOrderbookResponse>> GetLiveOrderBookBySymbol(string symbol)
        {
            var result = await _exchangeDataUnifierService.GetLiveOrderBookBySymbol(symbol);
            return result;
        }
    }
}
