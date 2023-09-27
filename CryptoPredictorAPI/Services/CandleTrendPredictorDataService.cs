using CryptoPredictorAPI.Models;
using CryptoPredictorAPI.Services.IServices;

namespace CryptoPredictorAPI.Services
{
    public class CandleTrendPredictorDataService : ICandleTrendPredictorDataService
    {
        private readonly BinanceDbContext _dbContext;

        public CandleTrendPredictorDataService(BinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<BinanceKlineModel> GetHistoricalData()
        {
            return _dbContext.BinanceHistoricalData.OrderBy(data => data.OpenTime).ToList();
        }
        public double CalculatePercentageOfCloseDifference(decimal priceDifference, decimal lastClosePrice)
        {
            return (double)(priceDifference / lastClosePrice) * 100;
        }
    }
}