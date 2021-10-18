namespace CryptoIndexSeriesCaseStudyAPI
{
    public static class Constants
    {
        public static class Exchanges
        {
            public static string BINANCE = "Binance";
            public static string COINBASEPRO = "CoinBasePro";
            public static string HUOBI = "Huobi";
        }

        public static class Side
        {
            public static string BUY = "buy";
            public static string SELL = "sell";
        }

        public static class Intervals
        {
            public static string BinanceCandleStickDataOneDayInterval = "1d";
            public static int CoinBaseProCandleStickDataOneDayInterval = 86400;
        }

        public static int COINBASEPRO_ORDERBOOK_LEVEL = 2;
        public static string COINBASEPRO_SOCKET_URI = "wss://ws-feed-public.sandbox.pro.coinbase.com";
    }
}
