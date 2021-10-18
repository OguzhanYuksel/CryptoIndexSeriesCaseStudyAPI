using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoIndexSeriesCaseStudyAPI.DTOs
{
    public class UnifiedCandleStickDataItem : IUnifiedData
    {
        private readonly string _exchange;
        private readonly string _symbol;
        public UnifiedCandleStickDataItem(string exchange, string symbol)
        {
            _exchange = exchange;
            _symbol = symbol;
        }
        public string OpenTime { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
        public string Symbol => _symbol;
        public string Exchange => _exchange;
    }
}
