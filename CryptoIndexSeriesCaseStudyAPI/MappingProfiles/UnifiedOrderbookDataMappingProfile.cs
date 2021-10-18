using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Binance.Net.Interfaces;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot.MarketData;
using Coinbase.Pro.Models;
using CryptoExchange.Net.Sockets;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.Binance.OrderBook;
using CryptoIndexSeriesCaseStudyAPI.Clients.Responses.CoinBasePro.OrderBook;
using CryptoIndexSeriesCaseStudyAPI.DTOs;

namespace CryptoIndexSeriesCaseStudyAPI.MappingProfiles
{
    public class UnifiedOrderbookDataMappingProfile : Profile
    {
        public UnifiedOrderbookDataMappingProfile()
        {
            CreateMap<BinanceOrderBookResponse, UnifiedOrderbookResponse>().ConstructUsingServiceLocator();
            CreateMap<BinanceOrderBookDataItem, UnifiedOrderbookDataItem>();

            CreateMap<CoinBaseProOrderBookResponse, UnifiedOrderbookResponse>().ConstructUsingServiceLocator();
            CreateMap<CoinBaseProOrderBookDataItem, UnifiedOrderbookDataItem>();

            CreateMap<BinanceOrderBookEntry, UnifiedOrderbookDataItem>();
            CreateMap<BinanceEventOrderBook, UnifiedOrderbookResponse>().ConstructUsingServiceLocator()
                .ForMember(dest => dest.Asks, opt =>
                {
                    opt.MapFrom(source => source.Asks); 

                })
                .ForMember(dest => dest.Bids, opt =>
                {
                    opt.MapFrom(source => source.Bids);

                });

            CreateMap<OrderLiquidity, UnifiedOrderbookDataItem>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Size));
            CreateMap<SnapshotEvent, UnifiedOrderbookResponse>().ConstructUsingServiceLocator()
                .ForMember(dest => dest.Asks, opt =>
                {
                    opt.MapFrom(source => source.Asks);
                })
                .ForMember(dest => dest.Bids, opt =>
                {
                    opt.MapFrom(source => source.Bids);
                });
        }
    }
}
