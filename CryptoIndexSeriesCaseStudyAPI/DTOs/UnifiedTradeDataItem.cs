using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoIndexSeriesCaseStudyAPI.DTOs
{
    public class UnifiedTradeDataItem : IUnifiedData
    {
        private readonly string _exchange;
        private readonly string _symbol;
        public UnifiedTradeDataItem(string exchange, string symbol)
        {
            _exchange = exchange;
            _symbol = symbol;
        }

        public string Exchange => _exchange;
        public string Id { get; set; }
        public long TimeStamp { get; set; }
        public string DateTime { get; set; }
        public string Symbol => _symbol;
        public string Type { get; set; }
        public string Side { get; set; }
        public decimal Price { get; set; }
        public string Amount { get; set; }
        public decimal Total { get; set; }
    }
}
