using CryptoPredictorApi.Models;

namespace CryptoPredictorApi.Services.IServices
{
    public interface IDataService
    {
        List<BinanceKlineModel> GetHistoricalData();
        double CalculatePercentageOfCloseDifference(decimal priceDifference, decimal lastClosePrice);
    }
}