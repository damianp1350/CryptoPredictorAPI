using CryptoPredictorAPI.Models;

namespace CryptoPredictorAPI.Services.IServices
{
    public interface ICandleTrendPredictorDataService
    {
        double CalculatePercentageOfCloseDifference(decimal priceDifference, decimal lastClosePrice);
        List<BinanceKlineModel> GetHistoricalData();
    }
}