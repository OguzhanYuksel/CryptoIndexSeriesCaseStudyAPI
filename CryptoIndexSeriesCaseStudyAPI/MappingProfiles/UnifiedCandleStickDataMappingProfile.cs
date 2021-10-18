using AutoMapper;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.CandleStick;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.CandleStick;
using CryptoIndexSeriesCaseStudyAPI.DTOs;

namespace CryptoIndexSeriesCaseStudyAPI.MappingProfiles
{
    public class UnifiedCandleStickDataMappingProfile : Profile
    {
        public UnifiedCandleStickDataMappingProfile()
        {
            CreateMap<BinanceCandleStickDataItem, UnifiedCandleStickDataItem>().ConstructUsingServiceLocator();
            CreateMap<CoinBaseProCandleStickDataItem, UnifiedCandleStickDataItem>().ConstructUsingServiceLocator();
        }
    }
}
