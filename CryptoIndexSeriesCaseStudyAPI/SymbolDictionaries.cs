using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoIndexSeriesCaseStudyAPI.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoIndexSeriesCaseStudyAPI
{
    public static class SymbolDictionaries
    {
        public static Dictionary<string, string> BinanceSymbolDictionary;
        public static Dictionary<string, string> CoinBaseProSymbolDictionary;
        public static Dictionary<string, string> HuobiSymbolDictionary;
        public static async Task<IHost> InitializeSymbolDictionaries(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var binanceClient = scope.ServiceProvider.GetRequiredService<BinanceClient>();
                var coinBaseProClient = scope.ServiceProvider.GetRequiredService<CoinBaseProClient>();

                var binanceSymbols = await binanceClient.GetSymbols();
                var coinBaseProSymbols = await coinBaseProClient.GetSymbols();

                var coinBaseProSpecificSymbols = new List<CoinBaseProClient.CoinBaseProSymbolItem>();
                var binanceSpecificSymbols = new List<BinanceClient.BinanceSymbolItem>();
                var CommonSymbols = new List<SymbolPair>();

                foreach (var item in coinBaseProSymbols)
                {
                    var binanceCommonSymbol = binanceSymbols.Symbols.FirstOrDefault(x => x.Symbol == item.Id.Replace("-", ""));
                    if (binanceCommonSymbol != null)
                    {

                        CommonSymbols.Add(new SymbolPair()
                        {
                            BinanceSymbol = binanceCommonSymbol.Symbol,
                            CoinBaseProSymbol = item.Id
                        });
                    }
                    else
                    {
                        coinBaseProSpecificSymbols.Add(item);
                    }
                }

                binanceSpecificSymbols.AddRange(binanceSymbols.Symbols.Where(x => !CommonSymbols.Select(y => y.BinanceSymbol).Contains(x.Symbol)));

                await Task.Run(() =>
                {
                    BinanceSymbolDictionary = new Dictionary<string, string>();
                    CoinBaseProSymbolDictionary = new Dictionary<string, string>();
                    HuobiSymbolDictionary = new Dictionary<string, string>();

                    foreach (var commonSymbol in CommonSymbols)
                    {
                        BinanceSymbolDictionary.Add(commonSymbol.BinanceSymbol, commonSymbol.BinanceSymbol);
                        CoinBaseProSymbolDictionary.Add(commonSymbol.BinanceSymbol, commonSymbol.CoinBaseProSymbol);
                    }

                    foreach (var coinBaseProSpecificSymbol in coinBaseProSpecificSymbols)
                    {
                        CoinBaseProSymbolDictionary.Add(coinBaseProSpecificSymbol.Id, coinBaseProSpecificSymbol.Id);
                    }

                    foreach (var binanceSpecificSymbol in binanceSpecificSymbols)
                    {
                        BinanceSymbolDictionary.Add(binanceSpecificSymbol.Symbol, binanceSpecificSymbol.Symbol);
                    }
                });
            }
            return webHost;
        }
    }

    public class SymbolPair
    {
        public string BinanceSymbol { get; init; }
        public string CoinBaseProSymbol { get; init; }
    }
}
