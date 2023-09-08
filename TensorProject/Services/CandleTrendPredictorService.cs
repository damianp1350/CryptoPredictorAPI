using TensorProject.Services.IServices;

namespace TensorProject.Services
{
    public class CandleTrendPredictorService : ICandleTrendPredictorService
    {
        private readonly BinanceDbContext _dbContext;

        public CandleTrendPredictorService(BinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CountConsecutiveNegativeCandles()
        {
            var historicalData = _dbContext.BinanceHistoricalData.OrderByDescending(data => data.OpenTime).ToList();
            int negativeStreak = 0;

            foreach (var data in historicalData)
            {
                if (data.Close < data.Open)
                {
                    negativeStreak++;
                }
                else
                {
                    break;
                }
            }

            return negativeStreak;
        }

        public double CalculatePriceRiseProbability(int negativeStreak, decimal pips)
        {
            var historicalData = _dbContext.BinanceHistoricalData.OrderByDescending(data => data.OpenTime).ToList();
            int matchingSequences = 0;
            int priceRiseOccurrences = 0;

            for (int i = 0; i < historicalData.Count - negativeStreak; i++)
            {
                var subset = historicalData.Skip(i).Take(negativeStreak).ToList();
                if (subset.All(data => data.Close < data.Open))
                {
                    matchingSequences++;
                    var nextCandle = historicalData[i + negativeStreak];
                    if ((nextCandle.High - nextCandle.Open) >= pips)
                    {
                        priceRiseOccurrences++;
                    }
                }
            }

            return matchingSequences > 0 ? (double)priceRiseOccurrences / matchingSequences : 0;
        }
    }
}
