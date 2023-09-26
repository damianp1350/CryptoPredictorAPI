using CryptoPredictorApi.Models;
using CryptoPredictorApi.Services.IServices;

namespace CryptoPredictorApi.Services
{
    public class CandlePatternAnalyzerService : ICandlePatternAnalyzerService
    {
        private readonly BinanceDbContext _dbContext;
        private readonly int _analysisInterval = 10;

        public CandlePatternAnalyzerService(BinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Dictionary<string, string> CalculateNextCandleProbabilities()
        {
            var historicalData = _dbContext.BinanceHistoricalData.ToList();
            var patternCounts = new Dictionary<string, int>();
            var nextPatternCounts = new Dictionary<string, Dictionary<string, int>>();

            for (int i = 0; i <= historicalData.Count - _analysisInterval; i += _analysisInterval)
            {
                var subset = historicalData.Skip(i).Take(_analysisInterval).ToArray();
                var pattern = AnalyzeCandlePattern(subset);

                if (patternCounts.ContainsKey(pattern))
                {
                    patternCounts[pattern]++;
                }
                else
                {
                    patternCounts[pattern] = 1;
                }

                var nextSubset = historicalData.Skip(i + _analysisInterval).Take(_analysisInterval).ToArray();
                var nextPattern = AnalyzeCandlePattern(nextSubset);

                if (!nextPatternCounts.ContainsKey(pattern))
                {
                    nextPatternCounts[pattern] = new Dictionary<string, int>();
                }

                if (nextPatternCounts[pattern].ContainsKey(nextPattern))
                {
                    nextPatternCounts[pattern][nextPattern]++;
                }
                else
                {
                    nextPatternCounts[pattern][nextPattern] = 1;
                }
            }

            var patternProbabilities = new Dictionary<string, string>();

            foreach (var pattern in patternCounts.Keys)
            {
                if (nextPatternCounts.ContainsKey(pattern))
                {
                    var totalNextPatterns = nextPatternCounts[pattern].Values.Sum();
                    var probabilities = nextPatternCounts[pattern]
                        .Select(kv => $"{kv.Key}: {((double)kv.Value / totalNextPatterns) * 100:F2}%")
                        .ToList();
                    patternProbabilities[pattern] = string.Join(", ", probabilities);
                }
            }
            return patternProbabilities;
        }

        private string AnalyzeCandlePattern(BinanceKlineModel[] subset)
        {
            int greenCount = subset.Count(candle => candle.Close > candle.Open);
            int redCount = _analysisInterval - greenCount;

            return $"{greenCount} Green, {redCount} Red";
        }
    }
}
