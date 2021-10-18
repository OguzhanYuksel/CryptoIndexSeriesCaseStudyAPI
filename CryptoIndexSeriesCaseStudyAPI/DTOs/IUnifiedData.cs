using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoIndexSeriesCaseStudyAPI.DTOs
{
    public interface IUnifiedData
    {
        public string Exchange { get; }
        public string Symbol { get; }
    }
}
