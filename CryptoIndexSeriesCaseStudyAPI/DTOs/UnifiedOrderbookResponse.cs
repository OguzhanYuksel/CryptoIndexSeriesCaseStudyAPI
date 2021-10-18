using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoIndexSeriesCaseStudyAPI.DTOs
{
    public class UnifiedOrderbookResponse : IUnifiedData
    {
        private readonly string _exchange;
        private readonly string _symbol;
        public UnifiedOrderbookResponse(string exchange, string symbol)
        {
            _exchange = exchange;
            _symbol = symbol;
        }
        public List<UnifiedOrderbookDataItem> Bids { get; set; }
        public List<UnifiedOrderbookDataItem> Asks { get; set; }
        public string Exchange => _exchange;
        public string Symbol => _symbol;
    }
}
