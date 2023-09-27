namespace CryptoPredictorAPI.Models
{
    public class BitcoinPriceOutput
    {
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public float Volume { get; set; }
        public float QuoteAssetVolume { get; set; }
        public int NumberOfTrades { get; set; }
        public float TakerBuyBaseAssetVolume { get; set; }
        public float TakerBuyQuoteAssetVolume { get; set; }
    }
}
