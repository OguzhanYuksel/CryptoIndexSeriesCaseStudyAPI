using System;
using AutoMapper;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.Trade;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.Trade;
using CryptoIndexSeriesCaseStudyAPI.DTOs;

namespace CryptoIndexSeriesCaseStudyAPI.MappingProfiles
{
    public class UnifiedTradeDataMappingProfile : Profile
    {
        public UnifiedTradeDataMappingProfile()
        {
            CreateMap<BinanceTradeItem, UnifiedTradeDataItem>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(source => source.Qty))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(source => source.Time))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => UnixTimeStampToDateTimeString(source.Time)))
                .ForMember(dest => dest.Side, opt => opt.MapFrom(source => source.IsBuyerMaker ? Constants.Side.BUY : Constants.Side.SELL))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => source.QuoteQty))
                .ConstructUsingServiceLocator();

            CreateMap<CoinBaseProTradeItem, UnifiedTradeDataItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.TradeId))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(source => DateTimeOffset.Parse(source.Time).ToUnixTimeSeconds()))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.Time))
                .ForMember(dest => dest.Side, opt => opt.MapFrom(source => source.Side))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(source => source.Size))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => decimal.Parse(source.Price) * decimal.Parse(source.Size)))
                .ConstructUsingServiceLocator();
        }

        private string UnixTimeStampToDateTimeString(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime().ToString("O");
            return result;
        }
    }
}
